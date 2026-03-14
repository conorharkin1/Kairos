<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-else class="calendar-wrapper">
        <VueCal v-bind="vueCalConfig" :events="events" />
        <div class="course-legend">
            <span class="legend-title">Courses</span>
            <div v-for="(course, index) in courses" :key="index" class="legend-item">
                <span class="legend-color" :class="`course-colour-${index % 6}`"></span>
                <span class="legend-label">{{ course.courseCode }}</span>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted, defineExpose } from 'vue'
import { VueCal } from 'vue-cal'
import 'vue-cal/style'
import api from '../services/api.js'
import { useToast } from 'vue-toastification'
const toast = useToast()
import { convertToUTC, convertToAppTimezone } from "@/utils/dayjsUtils";
import dayjs from 'dayjs'
import DisplayErrorMessage from '@/services/error'

const isLoading = ref(false)

const vueCalConfig = {
    hideWeekends: false,
    time: true,
    viewsBar: false,
    weekNumbers: false,
    timeFrom: 8 * 60,
    timeTo: 21 * 60,
    snapToInterval: 15,
    allDayEvents: true,
    timeStep: 30,
    startWeekOnToday: true,
}


const courses = ref([]);
const events = ref([]);

const fetchCourses = async () => {
    isLoading.value = true
    
    try {
        const response = await api.get('course')
        courses.value = response.data;
    } catch (error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while fetching the courses. Please try again.')
        }
    } finally {
        isLoading.value = false
    }
}

const refreshSchedule = async () => {
    try {
        isLoading.value = true
        const [assignmentsRes, scheduleRes] = await Promise.all([
            api.get('assignment/all'),
            api.get('scheduler/schedule'),
        ])
        
        events.value = [
            ...buildAssignmentEvents(assignmentsRes.data),
            ...(scheduleRes.data ? buildScheduleEvents(scheduleRes.data) : []),
        ]
    } catch (error) {
        console.error('Error fetching schedule:', error)
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while fetching the schedule. Please try again.')
        }
    } finally {
        isLoading.value = false
    }
}

const buildAssignmentEvents = (assignments) => {
    return assignments
        .filter(a => a.dueDate)
        .map(a => {
            const { start, end } = getFormattedDatesForCalendar(a.dueDate)
            return {
                allDay: true,
                start,
                end,
                title: a.name,
                class: getCustomClassesForAssignment(a).join(' '),
            }
        })
}

const buildScheduleEvents = (schedule) => {
    const validBlocks = schedule.blocks
        .filter(Boolean)
        .sort((a, b) => {
            if (a.assignmentName !== b.assignmentName)
                return a.assignmentName.localeCompare(b.assignmentName)
            return new Date(a.startTime) - new Date(b.startTime)
        })

    return mergeConsecutiveBlocks(validBlocks).map(block => ({
        start: convertToAppTimezone(block.startTime).toDate(),
        end: convertToAppTimezone(block.endTime).toDate(),
        title: block.name,
        class: `vuecal-schedule-block ${getScheduleBlockColorClass(block.courseId)}`,
        color: '#000000',
    }))
}

// Helpers

const mergeConsecutiveBlocks = (blocks) => {
    if (!blocks.length) return []

    const merged = []
    let current = { ...blocks[0] }

    for (let i = 1; i < blocks.length; i++) {
        const next = blocks[i]

        if ( next.assignmentName === current.assignmentName 
            && next.courseId === current.courseId 
            && new Date(next.startTime).getTime() === new Date(current.endTime).getTime()
        ) {
            current.endTime = next.endTime
        } else {
            merged.push(current)
            current = { ...next }
        }
    }

    merged.push(current)
    return merged
}

const getScheduleBlockColorClass = (courseId) => {
    const courseIndex = courses.value.findIndex(c => c.id === courseId || c.courseId === courseId);
    return `course-colour-${courseIndex % 6}`;
}

const getFormattedDatesForCalendar = (date) => {
    const end = convertToAppTimezone(date).toDate();
    const start = new Date(end);
    start.setHours(start.getHours() - 2);

    return { start, end };
}

const getCustomClassesForAssignment = (assignment) => {
    const classes = ['vuecal-assignment'];
    const now = dayjs();
    const dueDate = convertToAppTimezone(assignment.dueDate);
    const diffInHours = dueDate.diff(now, 'hour');

    if (assignment.isComplete) {
        classes.push('complete');
        return classes;
    }

    if (assignment.isPriority) {
        classes.push('priority');
    }

    if (diffInHours < 0) {
        classes.push('late');
    } else if (diffInHours <= 72) {
        classes.push('due-soon');
    } else {
        classes.push('upcoming');
    }

    return classes;
}

defineExpose({ refreshSchedule });

onMounted(async () => {
    isLoading.value = true
    await Promise.all([
        fetchCourses(),
        refreshSchedule()
    ]);
    isLoading.value = false
})

</script>

<style scoped lang="scss">
.calendar-wrapper {
    display: flex;
    flex-grow: 1;
    position: relative;
    width: 100%;
}

.course-legend {
    position: absolute;
    bottom: 1rem;
    right: 1rem;
    background: rgba(30, 30, 30, 0.9);
    border: 1px solid $primary-text-light;
    border-radius: 6px;
    padding: 0.5rem 0.75rem;
    display: flex;
    flex-direction: column;
    gap: 0.35rem;
    z-index: 10;
    max-width: 180px;
    font-size: 0.75rem;
}

.legend-title {
    color: $primary-text-light;
    font-weight: 600;
    font-size: 0.7rem;
    text-transform: uppercase;
    letter-spacing: 0.5px;
    margin-bottom: 0.25rem;
    opacity: 0.8;
}

.legend-item {
    display: flex;
    align-items: center;
    gap: 0.5rem;
}

.legend-color {
    width: 12px;
    height: 12px;
    border-radius: 2px;
    flex-shrink: 0;

    &.course-colour-0 {
        background-color: $pastel-green;
    }

    &.course-colour-1 {
        background-color: $pastel-purple;
    }

    &.course-colour-2 {
        background-color: $pastel-blue;
    }

    &.course-colour-3 {
        background-color: $pastel-yellow;
    }

    &.course-colour-4 {
        background-color: $pastel-pink;
    }

    &.course-colour-5 {
        background-color: $pastel-sea-green;
    }
}

.legend-label {
    color: $primary-text-light;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.vuecal {
    flex-grow: 1;
    border-left: 3px solid $primary-text-light;
    --vuecal-primary-color: #316191;
    --vuecal-secondary-color: #2e2e2e;
    --vuecal-base-color: #ffffff;
    --vuecal-contrast-color: #000000;

    --vuecal-border-color: color-mix(in srgb, var(--vuecal-base-color) 8%, transparent);

    --vuecal-header-color: var(--vuecal-base-color);
    --vuecal-event-color: var(--vuecal-base-color);
    --vuecal-event-border-color: currentColor;

    --vuecal-border-radius: 0px;
    --vuecal-height: 100%;
    --vuecal-min-schedule-width: 100%;
    --vuecal-min-cell-width: 80px;
    --vuecal-transition-duration: 0.3s;
}

.vuecal__cell-events {
    overflow-y: auto;
}

::v-deep(.vuecal__event-details .vuecal__event-title) {
    font-size: 1.2rem;
    font-weight: 600;
}

::v-deep(.vuecal__event-details .vuecal__event-time) {
    font-size: 1rem;
}

::v-deep(.vuecal__event.late) {
    --vuecal-primary-color: red;
}

::v-deep(.vuecal__event.due-soon) {
    --vuecal-primary-color: #{$primary-orange-colour};
}

::v-deep(.vuecal__event.complete) {
    --vuecal-primary-color: #{$primary-green-colour};
}

::v-deep(.vuecal__event.upcoming) {
    --vuecal-primary-color: #{$primary-blue-colour};
}

::v-deep(.vuecal__event.priority) {
    --vuecal-event-border-color: #{$primary-yellow-colour};
}

::v-deep(.vuecal__event.course-colour-0) {
    --vuecal-primary-color: #{$pastel-green};
}

::v-deep(.vuecal__event.course-colour-1) {
    --vuecal-primary-color: #{$pastel-purple};
}

::v-deep(.vuecal__event.course-colour-2) {
    --vuecal-primary-color: #{$pastel-blue};
}

::v-deep(.vuecal__event.course-colour-3) {
    --vuecal-primary-color: #{$pastel-yellow};
}

::v-deep(.vuecal__event.course-colour-4) {
    --vuecal-primary-color: #{$pastel-pink};
}

::v-deep(.vuecal__event.course-colour-5) {
    --vuecal-primary-color: #{$pastel-sea-green};
}

::v-deep(.vuecal__event.vuecal-schedule-block) {
    border-left: 3px solid var(--vuecal-primary-color);
    opacity: 0.9;
}
</style>
