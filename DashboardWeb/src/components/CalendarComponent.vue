<template>
    <div class="calendar-wrapper">
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
import { ref, onMounted, defineProps } from 'vue'
import { VueCal } from 'vue-cal'
import 'vue-cal/style'
import api from '../services/api.js'
import { useToast } from 'vue-toastification'
const toast = useToast()
import { convertToUTC, convertToAppTimezone } from "@/utils/dayjsUtils";
import dayjs from 'dayjs'

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

const props = defineProps({
    courses: Array,
})

const events = ref([]);

const fetchAssignments = async () => {
    try {
        const response = await api.get('assignment/all')
        buildCalendarEventsAssignments(response.data);
    } catch (error) {
        toast.error('Error fetching assignments')
    } finally {

    }
}

const fetchSchedule = async () => {
    try {
        const response = await api.get('scheduler/schedule')
        console.log(response.data);
        if (!response.data)
            return toast.info("Have you tried creating a schedule?")
        constructEventsFromSchedule(response.data);
    } catch (error) {
        toast.error('Error fetching schedule');
    } finally {

    }
}

const constructEventsFromSchedule = (schedule) => {
    const newEvents = [];

    // Filter out null blocks and sort by assignment and start time
    const validBlocks = schedule.blocks
        .filter(block => block)
        .sort((a, b) => {
            // First sort by assignment name, then by start time
            if (a.assignmentName !== b.assignmentName) {
                return a.assignmentName.localeCompare(b.assignmentName);
            }
            return new Date(a.startTime) - new Date(b.startTime);
        });

    // Merge consecutive blocks for the same assignment
    const mergedBlocks = mergeConsecutiveBlocks(validBlocks);

    mergedBlocks.forEach(block => {
        const start = convertToAppTimezone(block.startTime).toDate();
        const end = convertToAppTimezone(block.endTime).toDate();
        const colorClass = getScheduleBlockColorClass(block.courseId);

        newEvents.push({
            start: start,
            end: end,
            title: `${block.assignmentName}`,
            class: `vuecal-schedule-block ${colorClass}`,
            color: '#000000',
        });
    });
    events.value.push(...newEvents);
}

const mergeConsecutiveBlocks = (blocks) => {
    if (blocks.length === 0) return [];

    const merged = [];
    let currentMerged = { ...blocks[0] };

    for (let i = 1; i < blocks.length; i++) {
        const block = blocks[i];
        const currentEnd = new Date(currentMerged.endTime).getTime();
        const blockStart = new Date(block.startTime).getTime();

        // Check if this block is consecutive with the current merged block
        // and belongs to the same assignment
        if (block.assignmentName === currentMerged.assignmentName &&
            block.courseId === currentMerged.courseId &&
            blockStart === currentEnd) {
            // Extend the current merged block
            currentMerged.endTime = block.endTime;
        } else {
            // Push the current merged block and start a new one
            merged.push(currentMerged);
            currentMerged = { ...block };
        }
    }
    // Don't forget the last merged block
    merged.push(currentMerged);

    return merged;
}

const getScheduleBlockColorClass = (courseId) => {
    // Get course index from the courses prop for consistent coloring
    const courseIndex = props.courses.findIndex(c => c.id === courseId || c.courseId === courseId);
    // Use modulo to cycle through available colors
    const colorIndex = courseIndex % 6;
    return `course-colour-${colorIndex}`;
}

const buildCalendarEventsAssignments = (assignments) => {
    const allDayEvents = [];

    assignments.forEach(as => {
        if (!as.dueDate) {
            return;
        }

        const { start, end } = getFormattedDatesForCalendar(as.dueDate);
        const classes = getCustomClassesForAssignment(as);

        allDayEvents.push({
            allDay: true,
            start: start,
            end: end,
            title: as.name,
            class: classes.join(' '),
        });

    });
    events.value.push(...allDayEvents);
}

const getFormattedDatesForCalendar = (date) => {
    const end = convertToAppTimezone(date).toDate();
    const start = new Date(end);
    start.setHours(start.getHours() - 2);

    return { start: start, end: end };
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

onMounted(async () => {
    await fetchAssignments()
    await fetchSchedule()
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

.vuecal__title small {
    display: none !important;
}

.vuecal__cell-events {
    overflow-y: auto;
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
