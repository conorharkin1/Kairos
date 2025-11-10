<template>
    <vue-cal v-bind="vueCalConfig" :events="events" />
</template>

<script setup>
import { ref } from 'vue'
import { VueCal } from 'vue-cal'
import 'vue-cal/style'

const vueCalConfig = {
    hideWeekends: false,
    time: true,
    viewsBar: false,
    weekNumbers: false,
    timeFrom: 7 * 60,
    timeTo: 23 * 60,
    snapToInterval: 15,
}

const events = ref([
    {
        start: '2025-10-28 09:00',
        end: '2025-10-28 15:00',
        title: 'CSC3032 Project Work',
        content: 'Working on the dashboard project',
        class: 'custom-work-class',
    },
    {
        start: '2025-10-29 10:00',
        end: '2025-10-29 11:30',
        title: 'CSC3065 Quiz',
        content: 'Preparing for the quiz',
        class: 'custom-quiz-class',
    },
    {
        start: '2025-10-30 07:00',
        end: '2025-10-30 15:00',
        title: 'CSC3045 Study Time',
        content: 'Studying for the exam',
        class: 'custom-study-class',
    },
])
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
</style>
