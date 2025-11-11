<template>
    <div v-for="(courseId, index) in courses" :key="index" class="tab">
        {{ courseId }}
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../services/api.js'

const courses = ref([])

const fetchCourses = async () => {
    try {
        const response = await api.get('course/all/ids')
        courses.value = response.data
        console.log('Fetched courses:', response.data)
    } catch (error) {
        console.error('Error fetching courses:', error)
    }
}

onMounted(() => {
    fetchCourses()
})
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
