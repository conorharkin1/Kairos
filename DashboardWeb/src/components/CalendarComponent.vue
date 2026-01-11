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
    timeFrom: 7 * 60,
    timeTo: 23 * 60,
    snapToInterval: 15,
    allDayEvents: true,
}

const events = ref([
    {
        start: '2025-11-12 09:00',
        end: '2025-11-12 15:00',
        title: 'CSC3032 Project Work',
        content: 'Working on the dashboard project',
        class: 'custom-work-class',
    },
    {
        start: '2025-11-13 10:00',
        end: '2025-11-13 11:30',
        title: 'CSC3065 Quiz',
        content: 'Preparing for the quiz',
        class: 'custom-quiz-class',
    },
    {
        start: '2025-11-14 07:00',
        end: '2025-11-14 15:00',
        title: 'CSC3045 Study Time',
        content: 'Studying for the exam',
        class: 'custom-study-class',
    },
])

const fetchAssignments = async () => {
    try {
        const response = await api.get('assignment/all')
        buildCalendarEventsAssignments(response.data);
    } catch (error) {
        toast.error('Error fetching assignments')
    } finally {

    }
}

const buildCalendarEventsAssignments = (assignments) => {
    const formattedEvents = [];
    const allDayEvents = [];

    assignments.forEach(as => {
        // Skip assignments without due dates
        if (!as.dueDate) {
            return;
        }

        const {start, end} = getFormattedDatesForCalendar(as.dueDate);
        const classes = getCustomClassesForAssignment(as, end);

        formattedEvents.push({
            start: start,
            end: end,
            title: as.name,
            class: classes.join(' '),
        });

        allDayEvents.push({
            allDay: true,
            start: start,
            end: end,
            title: as.name,
            class: classes.join(' '),
        });

    });
    events.value.push(...formattedEvents);
    events.value.push(...allDayEvents);
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

onMounted(async () => {
    await fetchAssignments()
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
