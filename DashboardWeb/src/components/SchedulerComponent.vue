<template>
    <div class="scheduler-landing-container">
        <Icon icon="teenyicons:loader-solid" width="100" height="100" class="spinning" :class="{ 'constant-spinning': runningScheduler }"/>
        <button v-if="!runningScheduler" @click="runScheduler" class="btn-primary btn-good">Create a schedule</button>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import api from '@/services/api.js'

const runningScheduler = ref(false)

const runScheduler = async () => {
    runningScheduler.value = true
    
    try {
        await api.post('/scheduler')
    } catch (error) {
        console.error('Error running scheduler:', error)
    } finally {
        runningScheduler.value = false
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

@keyframes spinning {
    from {
        transform: rotate(0deg);
    }

    to {
        transform: rotate(360deg);
    }
}
</style>
