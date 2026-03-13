<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-for="(course, index) in courses" :key="index" @click="changeCourse(course.id)" class="tab btn-primary" :class="{ 'selected': course.id === currentCourse }">
        <h3>{{ course.courseCode }}</h3>
    </div>
        <button v-if="!showCreate" class="btn btn-primary btn-good create-course" @click="toggleShowCreate">Create Course</button>
        <button v-if="currentCourse != 0" class="btn btn-primary btn-danger delete-course" @click="deleteCourse">Delete Course</button>
        <div v-if="showCreate" class="create-inputs">
            <input v-model="newCourseName" type="text" placeholder="Course Name" class="input" />
            <input v-model="newCourseCode" type="text" placeholder="Course Code" class="input" />
            <button class="btn btn-primary btn-good" @click="createCourse">Create</button>
            <button class="btn btn-primary btn-danger" @click="toggleShowCreate">Cancel</button>
        </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../services/api.js'
import { useToast } from 'vue-toastification'
const toast = useToast()
const isLoading = ref(false)

const currentCourse = ref(0)
const courses = ref([])

const showCreate = ref(false)
const newCourseName = ref('')
const newCourseCode = ref('')

const createCourse = async () => {
    if (!newCourseName.value || !newCourseCode.value) {
        toast.error('Please enter both a course name and course code.')
        return
    }
    
    isLoading.value = true
    try {
        const response = await api.post('course', {
            name: newCourseName.value,
            courseCode: newCourseCode.value,
        })
        changeCourse(response.data.id)
        toast.success('Course created successfully!')
    } catch (error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while creating the course. Please try again.')
        }
    } finally {
        isLoading.value = false
        toggleShowCreate()
        await fetchCourses()
    }
}

const deleteCourse = async () => {
    if (!currentCourse.value) {
        toast.error('No course selected to delete.')
        return
    }
    
    isLoading.value = true
    try {
        await api.delete(`course/${currentCourse.value}`)
        changeCourse(0)
        toast.success('Course deleted successfully!')
    } catch (error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while deleting the course. Please try again.')
        }
    } finally {
        isLoading.value = false
        await fetchCourses()
    }
}

const fetchCourses = async () => {
    isLoading.value = true
    
    try {
        const response = await api.get('course')
        courses.value = response.data;
        emit('set-courses', response.data)
    } catch (error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while fetching the courses. Please try again.')
        }
    } finally {
        isLoading.value = false
    }
}

const changeCourse = (courseId) => {
    if (courseId && courseId !== currentCourse.value) {
        currentCourse.value = courseId
    }
    emit('set-current-course', courseId)
}

const toggleShowCreate = () => {
    showCreate.value = !showCreate.value
}

const emit = defineEmits(['set-current-course', 'set-courses'])

onMounted(() => {
    fetchCourses()
})

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

.create-inputs {
    margin-left: 1rem;
    text-align: center;
    display: flex;
    justify-content: center;
    flex-direction: row;
    gap: 0.5rem;
}

.create-inputs > input {
    font-size: 1rem;
    border: 2px solid $primary-text-light;
    outline: none;
    padding: 0.5rem;
    background-color: $primary-dark-background;
    color: $primary-text-light;
}

.create-course {
    margin-left: 1rem;
    width: 10rem;
}

.delete-course {
    margin-left: 1rem;
    width: 10rem;
}

</style>
