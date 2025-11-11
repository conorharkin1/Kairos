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
    <div class="dashboard-card-grid">
        <DashboardCard />
        <DashboardCard />
        <DashboardCard />
        <DashboardCard />
        <DashboardCard />
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import DashboardCard from './DashboardCardComponent.vue'
import api from '../services/api.js'

const assignments = ref([])

const getAssignments = async () => {
    try {
        const response = await api.get('api/assignments')
        assignments.value = response.data
    } catch (error) {
        console.error('Error fetching assignments:', error)
    }
}

onMounted(() => {
    getAssignments()
})

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
