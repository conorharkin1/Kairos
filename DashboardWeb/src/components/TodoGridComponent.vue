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
                        label="name" required></v-select>

                    <span>Select the Type of Task to Create</span>
                    <v-select v-model="selectedType" :options="types"></v-select>

                    <span>Title</span>
                    <input v-model="selectedTitle" type="text" placeholder="E.g. Write report..." />

                    <span>Due Date</span>
                    <input ref="dueDateInput" v-model="selectedDueDate" type="text" placeholder="Due date..." />
                </div>
                <div class="create-form-right">
                    <span v-if="selectedType === 'Quiz'">Lock Date</span>
                    <input v-if="selectedType === 'Quiz'" ref="lockDateInput" v-model="selectedLockDate" type="text" placeholder="Lock date..." />

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
                :htmlUrl="assignment.htmlUrl" @save="(updatedData) => updateAssignment(assignment.id, updatedData)" @delete="() => deleteAssignment(assignment.id)" 
            />
            <DashboardCard v-for="(quiz, index) in quizzes" :key="index" type="Quiz"
                :id="quiz.id" :title="quiz.name" :dueDate="quiz.dueDate" :lockdate="quiz.lockdate"
                :htmlUrl="quiz.htmlUrl" @save="(updatedData) => updateQuiz(quiz.id, updatedData)" @delete="() => deleteQuiz(quiz.id)" 
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
const lockDateInput = ref(null);
const selectedLockDate = ref('')
let dueDateFp = null;
let lockDateFp = null;
const selectedDescription = ref('')
const selectedUrl = ref('')

const getAssignmentsForCourse = async () => {
    isLoading.value = true

    try {
        const response = await api.get(`course/${props.currentCourse}/assignments`)
        assignments.value = response.data
    } catch (error) {
        toast.error('Error fetching assignments')
    } finally {
        isLoading.value = false
    }
}

const getQuizzesForCourse = async () => {
    isLoading.value = true

    try {
        const response = await api.get(`course/${props.currentCourse}/quizzes`)
        quizzes.value = response.data
        console.log(response.data)
    } catch (error) {
        toast.error('Error fetching quizzes')
    } finally {
        isLoading.value = false;
    }
}

const addNew = async () => {
    if (!selectedCourseId.value) {
        toast.error('Please select a course')
        return
    }
    
    if (!selectedType.value) {
        toast.error('Please select a task type')
        return
    }
    
    if (!selectedTitle.value || selectedTitle.value.trim() === '') {
        toast.error('Please enter a title')
        return
    }
    
    if (selectedType.value === 'Assignment' && (!selectedDueDate.value || selectedDueDate.value.trim() === '')) {
        toast.error('Please select a due date')
        return
    }

    isLoading.value = true

    switch (selectedType.value) {
        case 'Assignment':
            await createAssignment();
            isLoading.value = false;
            break
        case 'Quiz':
            await createQuiz();
            isLoading.value = false;
            break
        case 'Todo Item':
            isLoading.value = false;
            break
        default:
            isLoading.value = false
            break
    }
    
}

const createAssignment = async () => {
    try {
        const payload = {
            courseId: selectedCourseId.value,
            name: selectedTitle.value,
            dueDate: convertToUTC(selectedDueDate.value),
            description: selectedDescription.value,
            htmlUrl: selectedUrl.value,
        }
        await api.post('assignment', payload);
        
        await getAssignmentsForCourse()
        
        toast.success('Assignment created successfully')
        toggleCreateView()

    } catch (error) {
        toast.error('Error creating assignment')
    }
}

const updateAssignment = async (id, updatedData) => {
    isLoading.value = true;

    const payload = {
        name: updatedData.title,
        dueDate: convertToUTC(updatedData.dueDate),
        //description: updatedData.description,
        htmlUrl: updatedData.htmlUrl
    }

    try {
        await api.put(`assignment/${id}`, payload);
        toast.success(`Assignment updated successfully: ${updatedData.title}`)
        await getAssignmentsForCourse()
    } catch (error) {
        toast.error('Error updating assignment')
    } finally {
        isLoading.value = false;
    }
}

const deleteAssignment = async (id) => {
    isLoading.value = true;

    try {
        await api.delete(`assignment/${id}`)
        toast.success('Assignment deleted successfully')
        await getAssignmentsForCourse()
    } catch (error) {
        toast.error('Error deleting assignment:', error)
    } finally {
        isLoading.value = false;
    }
}

const createQuiz = async () => {
    try {
        const payload = {
            courseId: selectedCourseId.value,
            name: selectedTitle.value,
            dueDate: convertToUTC(selectedDueDate.value),
            lockDate: convertToUTC(selectedLockDate.value),
            htmlUrl: selectedUrl.value,
        }
        await api.post('quiz', payload);
        
        await getQuizzesForCourse()
        
        toast.success('Quiz created successfully')
        toggleCreateView()

    } catch (error) {
        toast.error('Error creating quiz')
    }
}

const updateQuiz = async (id, updatedData) => {
    isLoading.value = true;

    if(!updatedData.title || updatedData.title.trim() === '') {
        toast.error('Title cannot be empty')
        isLoading.value = false;
        return
    }

    const payload = {
        name: updatedData.title,
        dueDate: convertToUTC(updatedData.dueDate),
        lockDate: convertToUTC(updatedData.lockDate),
        htmlUrl: updatedData.htmlUrl
    }

    console.log('Updating quiz with payload:', payload);

    try {
        await api.put(`quiz/${id}`, payload);
        toast.success(`Quiz updated successfully: ${updatedData.title}`)
        await getQuizzesForCourse()
    } catch (error) {
        toast.error('Error updating quiz')
    } finally {
        isLoading.value = false;
    }
}

const deleteQuiz = async (id) => {
    isLoading.value = true;

    try {
        await api.delete(`quiz/${id}`)
        toast.success('Quiz deleted successfully')
        await getQuizzesForCourse()
    } catch (error) {
        toast.error('Error deleting quiz:', error)
    } finally {
        isLoading.value = false;
    }
}

const toggleCreateView = () => {
    creatingViewVisible.value = !creatingViewVisible.value
}

watch(() => props.currentCourse, () => {
    getAssignmentsForCourse();
    getQuizzesForCourse();
});

// Handle initialising / destroying the flatpickr instance.
watch(creatingViewVisible, async (visible) => {
    if (!visible) {
        dueDateFp?.destroy();
        return;
    }

    await nextTick();
    dueDateFp = flatpickr(dueDateInput.value, {
        enableTime: true,
        dateFormat: "Y-m-d H:i",
    });
});

watch(selectedType, async () => {
    await nextTick();

    lockDateFp?.destroy();

    if (selectedType.value === 'Quiz') {
        lockDateFp = flatpickr(lockDateInput.value, {
            enableTime: true,
            dateFormat: "Y-m-d H:i",
        });
    }
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
    max-width: 100%;
    flex-wrap: wrap;
    gap: 2rem;
    padding: 1rem;
    height: 100%;
    justify-content: center;
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
