<template>
    <div class="dashboard">
        <Header/>
        <StudyPartner/>
    </div>
</template>

<script setup>
import Header from '../components/Header.vue';
import StudyPartner from '../components/StudyPartner.vue';

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
    
</style>