<template>
    <div class="dashboard">
        <div class="calendar-view">
            <SideBar :sidebarOptionsLeft="sidebarOptionsLeft" side="left"/>
            <Calendar/>
            <SideBar :sidebarOptionsRight="sidebarOptionsRight" side="right"/>
        </div>
        <div class="course-tabs">
            <CourseTabs/>
        </div>
        <div class="todo-grid">
            <DashboardTodoGrid/>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue';
import SideBar from '../components/SideBar.vue';
import Calendar from '../components/Calendar.vue';
import CourseTabs from '../components/CourseTabs.vue';
import DashboardTodoGrid from '../components/DashboardTodoGrid.vue';
    // function fetchCourses() {
    //     fetch('http://localhost:5500/api/dashboard/courses')
    //         .then(response => response.json())
    //         .then(data => {
    //             console.log('Fetched courses:', data);
    //         })
    //         .catch(error => {
    //             console.error('Error fetching courses:', error);
    //         });
    // }
    
    // fetchCourses();
const sidebarOptionsLeft = [
    { icon: 'iconoir:profile-circle', height: 30, width: 30, tooltip: 'Profile' },
    { icon: 'ep:calendar', height: 30, width: 30, tooltip: 'Calendar' },
    { icon: 'uiw:dot-chart', height: 30, width: 30, tooltip: 'View Progress' }
];
const sidebarOptionsRight = [
    { icon: 'la:download', width: 30, height: 30, tooltip: 'Export Study Guide' },
    { icon: 'icon-park-outline:redo', width: 30, height: 30, tooltip: 'Redo Study Guide' }
];

</script>

<style scoped lang="scss">
    .dashboard {
        display: flex;
        flex-direction: column;
        background-color: $primary-background-dark;
    }
    .calendar-view {
        display: flex;
        flex-direction: row;
        height: 45vh;
        width: 100%;
    }
    .course-tabs {
        display: flex;
        overflow-x: auto;
        flex-direction: row;
        align-items: center;
        width: 100%;
        color: $primary-text-light;
        border: 2px solid $primary-text-light;
        text-wrap: nowrap;
        height: 4vh;
    }
    .todo-grid {
        display: flex;
        flex-direction: column;
        width: 100%;
        height: 45vh;
        margin-top: 0.5rem;
    }
</style>