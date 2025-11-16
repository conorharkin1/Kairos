<template>
    <div class="header">
        <div class="filter">
            <select>
                <option value="all">All Tasks</option>
                <option value="due-today">Due Today</option>
                <option value="overdue">Overdue</option>
                <option value="completed">Completed</option>
            </select>
        </div>
        <div class="search-bar">
            <input type="text" placeholder="Search tasks..." />
        </div>
    </div>
    <div v-if="assignments.length > 0" class="dashboard-card-grid">
        <DashboardCard
            v-for="(assignment, index) in assignments"
            :key="index"
            type="Assignment"
            :title="assignment.name"
        />
    </div>
    <div v-else>
        <h2>Please select a course to view your upcoming tasks</h2>
    </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import DashboardCard from './DashboardCardComponent.vue'
import api from '../services/api.js'

const props = defineProps({
    currentCourse: Number,
})

const assignments = ref([])
const quizzes = ref([])

const getAssignmentsForCourse = async () => {
    try {
        const response = await api.get(`course/${props.currentCourse}/assignments`)
        assignments.value = response.data
    } catch (error) {
        console.error('Error fetching assignments:', error)
    }
}

const getQuizzesForCourse = async () => {
    try {
        const response = await api.get(`course/${props.currentCourse}/quizzes`)
        quizzes.value = response.data
    } catch (error) {
        console.error('Error fetching quizzes:', error)
    }
}

watch(
    () => props.currentCourse,
    () => {
        getAssignmentsForCourse()
    }
)

</script>

<style scoped lang="scss">
.header {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 1rem;
    margin-bottom: 1rem;
    margin-top: 1rem;
}
.filter select {
    background-color: $primary-background-dark;
    color: $primary-text-light;
    padding: 0.5rem;
    font-size: 1rem;
}
.search-bar input {
    background-color: $primary-background-dark;
    color: $primary-text-light;
    padding: 0.5rem;
    font-size: 1rem;
    border: 2px solid $primary-text-light;
    outline: none;
}
.dashboard-card-grid {
    display: flex;
    flex-direction: row;
    overflow-x: auto;
    gap: 1rem;
    height: 100%;
}
</style>
