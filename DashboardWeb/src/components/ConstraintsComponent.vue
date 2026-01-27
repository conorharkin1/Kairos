<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-else class="global-constraints-container">
        <div class="daily-capacity-container">
            <h2>Daily Work Capacity</h2>
            <label class="info">How many hours can you work each day</label>
            <div class="sliders">
                <div class="slider" v-for="constraint in dailyCapacities" :key="constraint.dayIndex">
                    <label>{{ dayName(constraint.dayIndex)}}</label>
                    <br>
                    <label>{{ constraint.maxHours }} hours</label>
                    <Slider :min="0" :max="12" :step="1" v-model="constraint.maxHours" @hasUnsavedChanges="unsavedChanges = true" />
                </div>
            </div>
        </div>
        <div class="course-weight-container">
            <h2>Course Weights</h2>
            <label class="info">Give a higher percentage to courses you want to prioritise</label>
            <div class="sliders">
                <div class="slider" v-for="weight in courseWeights" :key="weight.courseId">
                    <label>{{ getCourseNameById(weight.courseId) }}</label>
                    <br>
                    <label>{{ weight.weight }}%</label>
                    <Slider :min="0" :max="100" :step="10" v-model="weight.weight" @hasUnsavedChanges="unsavedChanges = true" />
                </div>
            </div>
        </div>
        <div class="scheduler-rules-container">
            <h2>Scheduler Rules</h2>
            <label class="info">The session length controls how much time you will work in consecutive chunks</label>
            <div class="sliders">
                <div class="slider">
                    <label>Minimum Session Length</label>
                    <br>
                    <label>{{ schedulerRules.minSessionLength }} minutes</label>
                    <Slider :min="30" :max="240" :step="30" v-model="schedulerRules.minSessionLength" @hasUnsavedChanges="unsavedChanges = true" />
                </div>
                <div class="slider">
                    <label>Maximum Session Length</label>
                    <br>
                    <label>{{ schedulerRules.maxSessionLength }} minutes</label>
                    <Slider :min="30" :max="240" :step="30" v-model="schedulerRules.maxSessionLength" @hasUnsavedChanges="unsavedChanges = true" />
                </div>
            </div>
        </div>
        <div v-if="unsavedChanges" class="save-btn-container">
            <button class="btn btn-primary btn-good" @click="saveConstraints">Save Constraints</button>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import Slider from './Slider.vue';
import api from '../services/api.js'
import { useToast } from 'vue-toastification'
const toast = useToast()

const isLoading = ref(false)
const unsavedChanges = ref(false)

const courses = ref([])
const dailyCapacities = ref([])
const courseWeights = ref([])
const schedulerRules = ref({
    minSessionLength: 30,
    maxSessionLength: 180
})

const getCourseNameById = (id) => {
    const course = courses.value.find(c => c.id === id);
    return course.name;
}

const dayName = (index) =>
  ['Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday'][index]

const fetchCourses = async () => {
    try {
        const response = await api.get('course')
        courses.value = response.data;
    } catch (error) {
        toast.error('Error fetching courses');
    }
}

const fetchDailyCapacities = async () => {
    try {
        const response = await api.get('constraint/capacity')
        dailyCapacities.value = response.data;
        console.log(dailyCapacities.value);
    } catch (error) {
        if (error.response.status === 400) {
            toast.error(error.response.data.detail);
        }
        toast.error('Error fetching daily capacities');
    }
}

const fetchCourseWeights = async () => {
    try {
        const response = await api.get('constraint/weight')
        courseWeights.value = response.data;
        console.log(courseWeights.value);
    } catch (error) {
        if (error.response.status === 400) {
            toast.error(error.response.data.detail);
        }
        toast.error('Error fetching course weights');
    }
}

const fetchSchedulerRules = async () => {
    try {
        const response = await api.get('constraint/rules')
        schedulerRules.value = response.data;
        console.log(schedulerRules.value);
    } catch (error) {
        if (error.response.status === 400) {
            toast.error(error.response.data.detail);
        }
        toast.error('Error fetching scheduler rules');
    }
}

const saveConstraints = async () => {
    isLoading.value = true;
    await Promise.all([
        saveDailyCapacities(),
        saveCourseWeights(),
        saveSchedulerRules()
    ]);
    unsavedChanges.value = false;
    isLoading.value = false;
    toast.success('Constraints saved successfully');
}

const saveDailyCapacities = async () => {
    try {
        await api.put('constraint/capacity', dailyCapacities.value);
    } catch (error) {
        toast.error('Error saving daily capacities');
    }
}

const saveCourseWeights = async () => {
    try {
        await api.put('constraint/weight', courseWeights.value);
    } catch (error) {
        toast.error('Error saving course weights');
    }
}

const saveSchedulerRules = async () => {
    try {
        await api.put('constraint/rules', schedulerRules.value);
    } catch (error) {
        toast.error('Error saving scheduler rules');
    }
}

onMounted(async () => {
    isLoading.value = true
    await Promise.all([
        fetchCourses(),
        fetchDailyCapacities(),
        fetchCourseWeights(),
        fetchSchedulerRules()
    ])
    isLoading.value = false
})

</script>
 
<style scoped lang="scss">
.global-constraints-container {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
    color: $primary-text-light;
    padding: 1rem;
    gap: 3rem;
}
.daily-capacity-container,
.course-weight-container,
.scheduler-rules-container {
    padding: 1rem;
    border: 2px solid $primary-text-light;
    border-radius: 20px;
    box-shadow: 5px 5px 5px $primary-text-grey;
}

.sliders {
    display: flex;
    flex-direction: row;
    gap: 1rem;
    padding: 1rem;
    overflow-x: auto;
}

.slider {
    display: flex;
    flex-direction: column;
    flex-grow: 1;
    align-items: center;

    .slider-component {
        width: 100%;
    }
}

.info {
    color: $primary-text-grey;
}

.save-btn-container {
    margin-top: 2rem;
    display: flex;
    justify-content: center;
}
</style>