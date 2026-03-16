<template>
    <div class="scheduler-landing-container" v-if="!collectingFeedback">
        <Icon icon="teenyicons:loader-solid" width="100" height="100" class="spinning" :class="{ 'constant-spinning': runningScheduler }"/>
        <button v-if="!runningScheduler" @click="runScheduler" class="btn-primary btn-good">Create a schedule</button>
    </div>
    <div class="feedback-container" v-if="collectingFeedback">
        <div class="feedback-header">
            <h2>Based on the generated schedule...</h2>
            <h4>You can adjust how the scheduler prioritises assignment deadlines against your course preferences</h4>
        </div>
        <div class="settings-container">
            <div class="setting">
                <label>Deadline Urgency: {{schedulerRules.deadlineUrgencyWeighting}}</label>
            </div>
            <div class="slider">
                <Slider :min="0" :max="1" :step="0.1" v-model="schedulerRules.deadlineUrgencyWeighting" @hasUnsavedChanges="unsavedChanges = true" />
            </div>
            <div class="setting-last">
                <label>Course Preference: {{Math.round((1 - schedulerRules.deadlineUrgencyWeighting) * 10) / 10}}</label>
            </div>
        </div>
        <div class="actions">
            <button class="btn-primary btn-warning" @click="collectingFeedback = false">BACK</button>
            <button class="btn-primary btn-good" @click="saveFeedback">Save preference</button>
        </div>
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

const saveFeedback = async () => {
    try {
        /// ATTRIBUTION:
        /// The rounding logic was adapted from Stackoverflow
        /// URL: https://stackoverflow.com/questions/7342957/how-do-you-round-to-one-decimal-place-in-javascript
        schedulerRules.value.coursePreferenceWeighting = Math.round((1 - schedulerRules.value.deadlineUrgencyWeighting) * 10) / 10;
        await api.put('constraint/rules/feedback', schedulerRules.value)
        toast.success('Feedback provided! You will notice changes in the next generated schedule.')
        collectingFeedback.value = false;
    } catch (error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while providing feedback. Please try again.')
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
    min-width: 100%;
}

.feedback-container {
    display: flex;
    flex-direction: column;
    max-width: 100%;
    align-items: center;
    text-align: center;
    color: $primary-text-light;
    gap: 2rem;
    margin: 1rem;
    padding: 0.5rem;
}

.settings-container {
    display: flex;
    min-width: 100%;
    gap: 3rem;
    justify-content: space-between;
}

.setting {
    display: flex;
    flex-direction: column;
    text-align: start;
}

.setting-last {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
    text-align: end;
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
.actions {
    display: flex;
    min-width: 100%;
    gap: 10rem;
    justify-content: center;
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
