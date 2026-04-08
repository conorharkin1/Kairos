using DashboardServer.Data;
using DashboardServer.Models;
using DashboardServer.Repositories;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DashboardServer.Tests.Repository
{
    public class SchedulerRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SchedulerRepository _repository;
        private readonly Mock<IAssignmentRepository> _mockAssignmentRepo;
        private readonly Mock<ICourseRepository> _mockCourseRepo;
        private readonly IUserResolverService _userResolverService;

        public SchedulerRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userResolverMock = new Mock<IUserResolverService>();
            userResolverMock.Setup(x => x.UserId).Returns("123");

            _userResolverService = userResolverMock.Object;
            _dbContext = new ApplicationDbContext(options, _userResolverService);
            _mockAssignmentRepo = new Mock<IAssignmentRepository>();
            _mockCourseRepo = new Mock<ICourseRepository>();
            _repository = new SchedulerRepository(_dbContext, _mockAssignmentRepo.Object, _mockCourseRepo.Object);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task SaveSchedule_DeletesExistingSchedule()
        {
            // Arrange
            var userId = "123";

            // Create and save an existing schedule
            var existingSchedule = new Schedule(userId);
            var existingBlock = new ScheduleBlock(userId, courseId: 1, assignmentId: 1, name: "Existing Block", assignmentName: "Existing Assignment", priority: 1.0, blockType: BlockType.Assignment);
            existingBlock.SetSchedule(existingSchedule);
            existingSchedule.Blocks.Add(existingBlock);

            await _dbContext.Schedules.AddAsync(existingSchedule);
            await _dbContext.SaveChangesAsync();

            var existingScheduleId = existingSchedule.Id;

            // Verify existing schedule and block are in the database
            Assert.Equal(1, await _dbContext.Schedules.CountAsync());
            Assert.Equal(1, await _dbContext.ScheduleBlocks.CountAsync());

            // Build a new schedule matrix
            var newBlock = new ScheduleBlock(userId, courseId: 2, assignmentId: 2, name: "New Block", assignmentName: "New Assignment", priority: 2.0, blockType: BlockType.Assignment);
            var scheduleMatrix = new ScheduleBlock?[1, 1];
            scheduleMatrix[0, 0] = newBlock;

            // Act
            await _repository.SaveSchedule(userId, scheduleMatrix);

            // Assert
            var schedules = await _dbContext.Schedules
                .Include(s => s.Blocks)
                .ToListAsync();

            // Only one schedule should exist after saving
            Assert.Single(schedules);

            var savedSchedule = schedules.Single();

            // The original schedule should have been removed and replaced
            Assert.NotEqual(existingScheduleId, savedSchedule.Id);

            // All blocks should belong to the new schedule and the old block should be gone
            var blocks = await _dbContext.ScheduleBlocks.ToListAsync();
            Assert.Single(blocks);
            Assert.Equal(savedSchedule.Id, blocks[0].ScheduleId);
            Assert.Equal("New Block", blocks[0].Name);
        }
    }
}
