<template>
    <div v-for="course in courses" :key="course.id" class="tab">
        {{ course.name }}
    </div>
</template>

<script setup>
import { ref } from 'vue';

const courses = ref([
    { id: 1, name: 'CSC3032 Software Engineering Project' },
    { id: 2, name: 'CSC3065 Cloud Computing' },
    { id: 3, name: 'CSC3045 Team-based Software Innovation' },
    { id: 4, name: 'My custom course' },
    { id: 5, name: 'My other custom course' },
]);
</script>
<style scoped lang="scss">
    .tab {
        border-right: 2px solid $primary-text-light;
        cursor: pointer;
        padding: 0.5rem 4rem 0.5rem 4rem;
    }
    .tab:hover {
        color: $primary-red-outline;
    }
</style>