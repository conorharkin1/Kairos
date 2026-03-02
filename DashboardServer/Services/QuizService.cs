using AutoMapper;
using DashboardServer.DTOs.ModelDTOs.Requests;
using DashboardServer.DTOs.ModelDTOs.Responses;
using DashboardServer.Exceptions;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _repo;
        private readonly IMapper _mapper;

        public QuizService(IQuizRepository quizRepository, IMapper mapper)
        {
            _repo = quizRepository;
            _mapper = mapper;
        }

        public async Task<List<QuizResponseDTO>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            return _mapper.Map<List<QuizResponseDTO>>(result);
        }

        public async Task<QuizResponseDTO> CreateAsync(string userId, QuizRequestDTO dto)
        {
            Quiz quiz = new Quiz(userId, dto.CourseId, dto.Name, dto.DueDate, dto.LockDate, dto.HtmlUrl, dto.IsComplete, dto.IsPriority);

            var createdQuiz = await _repo.CreateAsync(quiz);
            return _mapper.Map<QuizResponseDTO>(createdQuiz);
        }

        public async Task CreateBulkAsync(string userId, List<QuizRequestDTO> quizzes)
        {
            List<Quiz> quizzesToCreate = quizzes.Select(dto => new Quiz(userId, dto.CourseId, dto.Name, dto.DueDate, dto.LockDate, dto.HtmlUrl, dto.IsComplete, dto.IsPriority)).ToList();

            await _repo.CreateBulkAsync(quizzesToCreate);
        }

        public async Task<QuizResponseDTO> UpdateAsync(string userId, int quizId, QuizRequestDTO dto)
        {
            Quiz quiz = await _repo.GetByIdAsync(quizId) ?? throw new NotFoundException("Quiz does not exist");

            if (quiz.UserId != userId)
            {
                throw new UnAuthorizedException("User is not authorized to update this quiz");
            }

            quiz.Update(dto.Name, dto.DueDate, dto.LockDate, dto.HtmlUrl, dto.IsComplete, dto.IsPriority);

            await _repo.Save();
            return _mapper.Map<QuizResponseDTO>(quiz);
        }

        public async Task DeleteAsync(string userId, int quizId)
        {
            var quiz = await _repo.GetByIdAsync(quizId) ?? throw new NotFoundException("Quiz does not exist");

            if (quiz.UserId != userId)
            {
                throw new UnAuthorizedException("User is not authorized to delete this quiz");
            }

            await _repo.DeleteAsync(quiz);
        }
    }
}
