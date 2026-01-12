<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-else class="global-constraints-container">
        <div class="global-sliders">
            <div class="daily-capacity-container" v-for="constraint in dailyCapacities" :key="constraint.dayIndex">
                <label>{{ dayName(constraint.dayIndex) }}</label>
                <Slider :min="0" :max="12" :step="1" v-model="constraint.maxHours" />
                <span>{{ constraint.maxHours }} hrs</span>
            </div>
            <h2>Daily Work Capacity sliders</h2>
            <h2>Course Priority sliders</h2>
            <h2>Min session length (recommended 30 minutes)</h2>
            <h2>Max session length (recommended 3 hours)</h2>
            <button @click="saveDailyCapacities">Testing</button>
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
const courses = ref([]);
const dailyCapacities = ref([])

const dayName = (index) =>
  ['Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday'][index]

const saveDailyCapacities = async () => {
    isLoading.value = true
    try {
        await api.put('constraint/capacity', dailyCapacities.value);
        toast.success('Daily capacities saved successfully');
    } catch (error) {
        toast.error('Error saving daily capacities');
    } finally {
        isLoading.value = false
    }
}
const updateDailyCapacity = (dayIndex, newMaxHours) => {
    var constraint = dailyCapacities.value[dayIndex]
    constraint.maxHours = newMaxHours;
}

const fetchCourses = async () => {
    isLoading.value = true
    
    try {
        const response = await api.get('course')
        courses.value = response.data;
    } catch (error) {
        toast.error('Error fetching courses');
    } finally {
        isLoading.value = false
    }
}

const fetchDailyCapacities = async () => {
    isLoading.value = true;

    try {
        const response = await api.get('constraint/capacity')
        response.data.length > 0 ? dailyCapacities.value = response.data : handleNoDailyCapacities();
        console.log('fetched daily capacities', dailyCapacities.value);
    } catch (error) {
        toast.error('Error fetching daily capacities');
    } finally {
        isLoading.value = false;
    }
}

const handleNoDailyCapacities = () => {
    dailyCapacities.value = [
        { dayIndex: 0, maxHours: 0 },
        { dayIndex: 1, maxHours: 0 },
        { dayIndex: 2, maxHours: 0 },
        { dayIndex: 3, maxHours: 0 },
        { dayIndex: 4, maxHours: 0 },
        { dayIndex: 5, maxHours: 0 },
        { dayIndex: 6, maxHours: 0 },
    ]
}

onMounted(async () => {
    await fetchCourses();
    await fetchDailyCapacities();
})

</script>

<style scoped lang="scss">
.global-constraints-container {
    display: flex;
    flex-direction: row;
    width: 100%;
    height: 100%;
}
.global-sliders {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
}
</style>