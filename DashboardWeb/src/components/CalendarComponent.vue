<template>
    <VueCal v-bind="vueCalConfig" :events="events" />    
</template>

<script setup>
import { ref, onMounted } from 'vue'
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
    timeStep: 30
}

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

    schedule.blocks.forEach(block => {
        if (block) {
            const start = convertToAppTimezone(block.startTime).toDate();
            const end = convertToAppTimezone(block.endTime).toDate();
            newEvents.push({
                start: start,
                end: end,
                title: String(block.assignmentId),
                class: 'vuecal-schedule-block',
            });
        }
    });
    events.value.push(...newEvents);
}

const buildCalendarEventsAssignments = (assignments) => {
    const allDayEvents = [];

    assignments.forEach(as => {
        if (!as.dueDate) {
            return;
        }

        const {start, end} = getFormattedDatesForCalendar(as.dueDate);
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
</style>
