<template>
    <div class="dashboard">
        <div class="study-guidance-view">
            <SideBar/>
            <Calendar/>
        </div>
        <div class="course-tabs">
            <CourseTabs/>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue';
import SideBar from '../components/SideBar.vue';
import Calendar from '../components/Calendar.vue';
import CourseTabs from '../components/CourseTabs.vue';
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
</script>

<style scoped lang="scss">
    .dashboard {
        display: flex;
        flex-direction: column;
        background-color: $primary-background-dark;
    }
    .study-guidance-view {
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
    }
</style>