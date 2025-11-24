<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-for="(course, index) in courses" :key="index" @click="$emit('change-course', course.id)" class="tab" :class="{ 'selected': course.id === currentCourse }">
        {{ course.courseCode }}
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../services/api.js'
import { useToast } from 'vue-toastification'

const toast = useToast()

const props = defineProps({
    currentCourse: Number,
})
const courses = ref([])
const isLoading = ref(false)

const fetchCourses = async () => {
    isLoading.value = true
    api.get('course')
        .then(response => {
            courses.value = response.data;
        })
        .catch(error => {
            toast.error('Error fetching courses:', error);
        })
        .finally(() => {
            isLoading.value = false
        });
}

onMounted(() => {
    fetchCourses()
})

const emit = defineEmits(['change-course'])
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

.tab.selected {
    color: $primary-red-outline;
}
</style>
