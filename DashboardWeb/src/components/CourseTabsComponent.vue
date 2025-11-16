<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-for="(courseId, index) in courses" :key="index" class="tab">
        {{ courseId }}
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api, { isLoading } from '../services/api.js'

const courses = ref([])

const fetchCourses = async () => {
    api.get('course/all/ids')
        .then(response => {
            courses.value = response.data;
            console.log('Fetched courses:', courses.value);
        })
        .catch(error => {
            console.error('Error fetching courses:', error);
        });
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
@keyframes l2 {to{background-size: 100% 3px}}
</style>
