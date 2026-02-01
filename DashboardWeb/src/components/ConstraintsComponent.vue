<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-else class="global-constraints-container">
        <div class="constraint-container">
            <h2>Daily Work Capacity</h2>
            <label class="info">How many hours can you work each day</label>
            <div class="sliders">
                <div class="slider" v-for="constraint in dailyCapacities" :key="constraint.dayIndex">
                    <label>{{ dayName(constraint.dayIndex) }}</label>
                    <br>
                    <label>{{ constraint.maxHours }} hours</label>
                    <Slider :min="0" :max="12" :step="1" v-model="constraint.maxHours"
                        @hasUnsavedChanges="unsavedChanges = true" />
                </div>
            </div>
        </div>
        <div class="constraint-container">
            <h2>Course Weights</h2>
            <label class="info">Give a higher percentage to courses you want to prioritise</label>
            <div class="sliders">
                <div class="slider" v-for="weight in courseWeights" :key="weight.courseId">
                    <label>{{ getCourseNameById(weight.courseId) }}</label>
                    <br>
                    <label>{{ weight.weight }}%</label>
                    <Slider :min="0" :max="100" :step="10" v-model="weight.weight"
                        @hasUnsavedChanges="unsavedChanges = true" />
                </div>
            </div>
        </div>
        <div class="constraint-container">
            <h2>Scheduler Rules</h2>
            <label class="info">The session length controls how much time you will work in consecutive chunks</label>
            <div class="sliders">
                <div class="slider disabled">
                    <label>Minimum Session Length</label>
                    <br>
                    <label>{{ schedulerRules.minSessionLength }} minutes</label>
                    <Slider :min="30" :max="240" :step="30" v-model="schedulerRules.minSessionLength"
                        @hasUnsavedChanges="unsavedChanges = true" />
                </div>
                <div class="slider">
                    <label>Maximum Session Length</label>
                    <br>
                    <label>{{ schedulerRules.maxSessionLength }} minutes</label>
                    <Slider :min="30" :max="240" :step="30" v-model="schedulerRules.maxSessionLength"
                        @hasUnsavedChanges="unsavedChanges = true" />
                </div>
            </div>
        </div>
        <div class="constraint-container">
            <h2>Unavailable Times</h2>
            <label class="info">The scheduler will avoid using any of the times provided here</label>
            <div class="avoid-period-container">
                <div class="avoid-constraint" v-for="constraint in avoidPeriods" :key="constraint.dayIndex">
                    <label>{{ dayName(constraint.dayIndex) }}</label>
                    <br>
                    <div class="time-input-row">
                        <flat-pickr v-model="constraint.newStartTime" :config="timeConfig" placeholder="From"
                            class="time-input" />

                        <flat-pickr v-model="constraint.newEndTime" :config="timeConfig" placeholder="To"
                            class="time-input" /> 

                        <button class="btn btn-primary btn-good" @click="addBlock(constraint); unsavedChanges = true">Add</button>
                    </div>
                    <div v-for="avoidPeriod in constraint.avoidTimeBlocks" :key="avoidPeriod.id">
                        <div class="schedule-block">
                            <label>{{ avoidPeriod.startTime }} - {{ avoidPeriod.endTime }}</label>
                            <button @click="removeBlock(constraint, avoidPeriod); unsavedChanges = true">x</button>
                        </div>
                    </div>
                    <div></div>
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
import flatPickr from 'vue-flatpickr-component';
import 'flatpickr/dist/flatpickr.css';
const toast = useToast()

const timeConfig = {
    enableTime: true,
    noCalendar: true,
    dateFormat: "H:i",
    time_24hr: true,
    minuteIncrement: 30
};

const isLoading = ref(false)
const unsavedChanges = ref(false)

const courses = ref([])
const dailyCapacities = ref([])
const courseWeights = ref([])
const schedulerRules = ref({
    minSessionLength: 30,
    maxSessionLength: 180
})
const avoidPeriods = ref([])

const getCourseNameById = (id) => {
    const course = courses.value.find(c => c.id === id);
    return course.name;
}

const dayName = (index) =>
    ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'][index]

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
    } catch (error) {
        if (error.response.status === 400) {
            toast.error(error.response.data.detail);
        }
        toast.error('Error fetching scheduler rules');
    }
}

const fetchAvoidPeriods = async () => {
    try {
        const response = await api.get('constraint/avoidperiod')
        avoidPeriods.value = response.data;
    } catch (error) {
        if (error.response.status === 400) {
            toast.error(error.response.data.detail);
        }
        toast.error('Error fetching avoid periods');
    }
}

const saveConstraints = async () => {
    isLoading.value = true;
    await Promise.all([
        saveDailyCapacities(),
        saveCourseWeights(),
        saveSchedulerRules(),
        saveAvoidPeriods()
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

const saveAvoidPeriods = async () => {
    try {
        await api.put('constraint/avoidperiod', avoidPeriods.value);
    } catch (error) {
        toast.error('Error saving avoid periods');
    }
}

const addBlock = (constraint) => {
    if (!constraint.newStartTime || !constraint.newEndTime) return;

    constraint.avoidTimeBlocks.push({
        id: 1,
        startTime: constraint.newStartTime,
        endTime: constraint.newEndTime,
    });

    constraint.newStartTime = '';
    constraint.newEndTime = '';
};

const removeBlock = (constraint, block) => {
    const index = constraint.avoidTimeBlocks.indexOf(block);
    if (index > -1) {
        constraint.avoidTimeBlocks.splice(index, 1);
    }
};

onMounted(async () => {
    isLoading.value = true
    await Promise.all([
        fetchCourses(),
        fetchDailyCapacities(),
        fetchCourseWeights(),
        fetchSchedulerRules(),
        fetchAvoidPeriods()
    ]);
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
    overflow-x: hidden;
    box-sizing: border-box;
}

.avoid-period-container {
    display: flex;
    flex-direction: row;
    gap: 1rem;
    padding: 1rem;
    overflow-x: auto;
}

.avoid-constraint {
    display: flex;
    flex-direction: column;
    flex: 1 1 0;
    min-width: 0;

    .time-input-row {
        width: 100%;
    }
}

.constraint-container {
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

.time-input-row {
    display: flex;
    flex-direction: row;
    gap: 0.5rem;
    margin-bottom: 0.5rem;
    max-width: 100%;
    min-width: 0;
    flex-wrap: wrap;
}

.time-input {
    padding: 0.5rem;
    border-radius: 5px;
    border: 1px solid $primary-text-light;
    background-color: $primary-grey-background;
    color: $primary-text-light;
    text-align: center;
    flex: 1 1 0;
    min-width: 50px;
    max-width: 80px;

    &::placeholder {
        color: $primary-text-grey;
    }
}

.schedule-block {
    display: flex;
    border: 1px solid $primary-text-light;
    background-color: $primary-blue-colour;
    border-radius: 5px;
    padding: 0.2rem;
    text-align: center;
    color: $primary-text-dark;
    font-weight: 600;
    justify-content: space-evenly;
}

.disabled {
    opacity: 0.5;
    pointer-events: none;
}
</style>