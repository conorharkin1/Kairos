<template>
    <div class="scheduler-landing-container" v-if="!collectingFeedback">
        <Icon icon="teenyicons:loader-solid" width="100" height="100" class="spinning" :class="{ 'constant-spinning': runningScheduler }"/>
        <button v-if="!runningScheduler" @click="runScheduler" class="btn-primary btn-good">Create a schedule</button>
    </div>
    <div class="scheduler-landing-container" v-if="collectingFeedback">
        <h2>Based on the generated schedule...</h2>
        <h4>You can adjust how the scheduler prioritises assignment deadlines against your course preferences</h4>
        <div class="slider">
            <label>deadlineUrgencyWeighting</label>
            <label>{{schedulerRules.deadlineUrgencyWeighting}}</label>
            <br>
            <label>coursePreferenceWeighting</label>
            <label>{{1 - schedulerRules.deadlineUrgencyWeighting}}</label>
            <Slider :min="0" :max="1" :step="0.1" v-model="schedulerRules.deadlineUrgencyWeighting"
                            @hasUnsavedChanges="unsavedChanges = true" />
        </div>
        <button @click="collectingFeedback = false">BACK</button>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import api from '@/services/api.js'
import { useToast } from 'vue-toastification'
import Slider from './Slider.vue';

const toast = useToast()
const emit = defineEmits(['schedule-created'])
import DisplayErrorMessage from '@/services/error'

const runningScheduler = ref(false)
const collectingFeedback = ref(false)
const schedulerRules = ref();

const runScheduler = async () => {
    runningScheduler.value = true
    toast.info('Generating schedule...')
    
    try {
        await api.post('/scheduler')
        toast.success('Schedule created successfully!')
        emit('schedule-created')
        await fetchSchedulerRules();
        collectingFeedback.value = true;
    } catch (error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while running the scheduler. Please try again.')
        }
    } finally {
        runningScheduler.value = false
    }
}

const fetchSchedulerRules = async () => {
    try {
        const response = await api.get('constraint/rules')
        schedulerRules.value = response.data;
        console.log(schedulerRules.value);
    } catch (error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while fetching your scheduler rules. Please try again.')
        }
    }
}

</script>

<style scoped lang="scss">
.scheduler-landing-container {
    display: flex;
    flex-direction: column;
    flex: 1;
    align-items: center;
    color: $primary-text-light;
    gap: 3rem;
    margin: 2rem;
    padding: 0.5rem;
}



.spinning {
    display: inline-block;
    transform-origin: 50% 50%;
}

.spinning:hover {
    animation: spinning 5s linear infinite;
}

.constant-spinning {
    animation: spinning 5s linear infinite;
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

@keyframes spinning {
    from {
        transform: rotate(0deg);
    }

    to {
        transform: rotate(360deg);
    }
}
</style>
