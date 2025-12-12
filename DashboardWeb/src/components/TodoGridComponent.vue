<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-else>
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
                <input type="text" placeholder="Search ..." />
            </div>
            <button class="btn-primary btn-good" v-if="!creatingViewVisible" @click="toggleCreateView">Add New</button>
            <button class="btn-primary btn-danger" v-if="creatingViewVisible" @click="toggleCreateView">Cancel</button>
        </div>
        <div v-if="creatingViewVisible" class="create-view">
            <div class="create-form">
                <div class="create-form-left">
                    <span>Select the Course(s) this will belong to</span>
                    <v-select v-model="selectedCourseId" :options="courses" :reduce="course => course.id"
                        label="name"></v-select>

                    <span>Select the Type of Task to Create</span>
                    <v-select v-model="selectedType" :options="types"></v-select>

                    <span>Title</span>
                    <input v-model="selectedTitle" type="text" placeholder="E.g. Write report..." />

                    <span>Due Date</span>
                    <input ref="dueDateInput" v-model="selectedDueDate" type="text" placeholder="Due date..." />
                </div>
                <div class="create-form-right">
                    <span>Description</span>
                    <input v-model="selectedDescription" type="text" placeholder="E.g. Additional details..." />

                    <span>URL</span>
                    <input v-model="selectedUrl" type="text" placeholder="E.g. https://example.com" />

                    <div class="checkbox-row">
                        <span>Mark as favourite</span>
                        <input type="checkbox" />
                    </div>

                    <button class="btn-primary btn-good" @click="addNew">Save</button>
                </div>
            </div>
        </div>
        <div v-else-if="assignments.length > 0" class="dashboard-card-grid">
            <DashboardCard v-for="(assignment, index) in assignments" :key="index" type="Assignment"
                :id="assignment.id" :title="assignment.name" :dueDate="assignment.dueDate" :description="assignment.description"
                :htmlUrl="assignment.htmlUrl" @save="(updatedData) => saveAssignment(assignment.id, updatedData)" @delete="() => deleteAssignment(assignment.id)" 
            />
        </div>
        <div v-else-if="!props.currentCourse" class="no-selected-course">
            <h2>Please select a course to view your upcoming tasks</h2>
        </div>
        <div v-else class="no-selected-course">
            <h2>Nice, you have no upcoming work</h2>
        </div>
    </div>
</template>

<script setup>
import { ref, watch, onMounted, nextTick } from 'vue'
import DashboardCard from './DashboardCardComponent.vue'
import api from '../services/api.js'
import { useToast } from 'vue-toastification'
const toast = useToast()
import flatpickr from "flatpickr";
import 'flatpickr/dist/themes/dark.css';
import { convertToUTC, convertToAppTimezone } from "@/utils/dayjsUtils";

const props = defineProps({
    currentCourse: Number,
    courses: Array,
})

const assignments = ref([])
const quizzes = ref([])
const isLoading = ref(false)
const creatingViewVisible = ref(false)

// Create view inputs
const types = ['Assignment', 'Quiz', 'Todo Item']
const selectedCourseId = ref(null)
const selectedType = ref(types[0])
const selectedTitle = ref('')
const selectedDueDate = ref('')
const dueDateInput = ref(null);
let fp = null;
const selectedDescription = ref('')
const selectedUrl = ref('')

const getAssignmentsForCourse = async () => {
    try {
        isLoading.value = true
        const response = await api.get(`course/${props.currentCourse}/assignments`)
        assignments.value = response.data
    } catch (error) {
        toast.error('Error fetching assignments:', error)
    } finally {
        isLoading.value = false
    }
}

const getQuizzesForCourse = async () => {
    try {
        const response = await api.get(`course/${props.currentCourse}/quizzes`)
        quizzes.value = response.data
    } catch (error) {
        toast.error('Error fetching quizzes:', error)
    }
}

const saveAssignment = async (id, updatedData) => {
    try {
        await api.put(`assignment/${id}`, {
            name: updatedData.title,
            dueDate: convertToUTC(updatedData.dueDate),
            //description: updatedData.description,
            htmlUrl: updatedData.htmlUrl
        })
        .then(() => {
            toast.success(`Assignment updated successfully: ${updatedData.title}`)
            getAssignmentsForCourse()
        })
        .catch((error) => {
            toast.error('Error updating assignment:', error);
        })
        .finally(() => {
            isLoading.value = false
        });
    } catch (error) {
        toast.error('Error updating assignment:', error)
    }
}

const addNew = async () => {
    isLoading.value = true
    switch (selectedType.value) {
        case 'Assignment':
            await api.post('assignment', {
                courseId: selectedCourseId.value,
                name: selectedTitle.value,
                dueDate: convertToUTC(selectedDueDate.value),
                description: selectedDescription.value,
                htmlUrl: selectedUrl.value,
            })
                .then(response => {
                    getAssignmentsForCourse()
                    toast.success('Assignment created successfully!')
                    toggleCreateView()
                })
                .catch((error) => {
                    toast.error('Error creating assignment:', error);
                })
                .finally(() => {
                    isLoading.value = false
                });
            break
        case 'Quiz':
            break
        case 'Todo Item':
            break
        default:
            isLoading.value = false
            break
    }
    // Implement API call to create new task
}

const deleteAssignment = async (id) => {
    try {
        await api.delete(`assignment/${id}`)
        toast.success('Assignment deleted successfully')
        getAssignmentsForCourse()
    } catch (error) {
        toast.error('Error deleting assignment:', error)
    }
}

const toggleCreateView = () => {
    creatingViewVisible.value = !creatingViewVisible.value
}

watch(() => props.currentCourse, getAssignmentsForCourse);

watch(creatingViewVisible, async (visible) => {
    if (!visible) {
        fp?.destroy();
        return;
    }

    await nextTick();
    fp = flatpickr(dueDateInput.value, {
        enableTime: true,
        dateFormat: "Y-m-d H:i",
    });
});

</script>

<style scoped lang="scss">
.header {
    display: flex;
    flex-direction: row;
    align-items: center;
    gap: 1rem;
    margin-bottom: 1rem;
    margin-top: 1rem;
    padding-left: 1rem;
}

.filter select {
    background-color: $primary-dark-background;
    color: $primary-text-light;
    padding: 0.5rem;
    font-size: 1rem;
}

.search-bar input {
    background-color: $primary-dark-background;
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
    gap: 2rem;
    padding: 1rem;
    height: 100%;
}

.no-selected-course {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100%;
    color: $primary-text-light;
}

.header .btn-good {
    width: 10rem;
}

.header .btn-danger {
    width: 10rem;
}

.create-view {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    padding: 1rem;
    margin: 1rem;
    justify-content: center;
}

.create-form {
    display: flex;
    flex-direction: row;
    gap: 5rem;
}

.create-form-left {
    flex: 5;
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
}

.create-form-right {
    flex: 5;
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
}

.create-form-right .checkbox-row {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    justify-content: space-between;
    width: 60%;
}

.checkbox-row input[type="checkbox"] {
    width: 15px;
    height: 15px;
    accent-color: $primary-green-colour;
    transform: translateY(5px);
}

.create-form-left input,
.create-form-right input {
    font-size: 1rem;
    border: 2px solid $primary-text-light;
    outline: none;
    padding: 0.5rem;
    background-color: $primary-dark-background;
    color: $primary-text-light;
}

.create-form-left span,
.create-form-right span {
    margin-top: 0.5rem;
    font-size: large;
    color: $primary-text-light;
}

.create-form-left button,
.create-form-right button {
    margin-top: 2rem;
}

.v-select {
    border: 2px solid $primary-text-light;
}
</style>
