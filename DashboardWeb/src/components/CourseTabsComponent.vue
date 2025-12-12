<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-for="(course, index) in courses" :key="index" @click="$emit('change-course', course.id)" class="tab btn-primary" :class="{ 'selected': course.id === currentCourse }">
        <h3>{{ course.courseCode }}</h3>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../services/api.js'
import { useToast } from 'vue-toastification'

const toast = useToast()

const props = defineProps({
    currentCourse: Number,
    courses: Array,
})

const isLoading = ref(false)

const emit = defineEmits(['change-course'])
</script>

<style scoped lang="scss">
.tab {
    text-align: center;
    width: 10rem;
}
.tab:hover {
    color: $primary-red-colour;
    box-shadow: 0 4px 8px 0 $primary-red-colour;
}

.tab.selected {
    color: $primary-red-colour;
}
</style>
