<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-for="(course, index) in courses" :key="index" @click="$emit('change-course', course.id)" class="tab" :class="{ 'selected': course.id === currentCourse }">
        {{ course.courseCode }}
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api, { isLoading } from '../services/api.js'
import { useToast } from 'vue-toastification'

const toast = useToast()

const props = defineProps({
    currentCourse: Number,
})
const courses = ref([])

const fetchCourses = async () => {
    api.get('course')
        .then(response => {
            courses.value = response.data;
        })
        .catch(error => {
            toast.error('Error fetching courses:', error);
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

.loading-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-grow: 1;
}
.loader {
  width: fit-content;
  font-weight: bold;
  font-family: sans-serif;
  padding-bottom: 8px;
  background: linear-gradient(currentColor 0 0) 0 100%/0% 3px no-repeat;
  animation: l2 2s linear infinite;
  color: $primary-red-outline;
}
.loader:before {
  content:"Loading..."
}

.tab.selected {
    color: $primary-red-outline;
}
@keyframes l2 {to{background-size: 100% 3px}}
</style>
