<template>
    <div class="scheduler-landing-container" v-if="landingContainerVisible">
        <Icon icon="teenyicons:loader-solid" width="100" height="100" class="spinning" />
        <button @click="landingContainerVisible = false" class="btn-primary btn-good">Create a schedule</button>
    </div>
    <div class="scheduler-options" v-else>
        <h1>Scheduler Component Loaded</h1>
    </div>
</template>

<script setup>
import { ref } from 'vue'

const isLoading = ref(false)
const landingContainerVisible = ref(true)
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

.scheduler-options {
    display: flex;
    flex-direction: column;
    flex: 1;
    align-items: center;
    color: $primary-text-light;
}



.spinning {
    display: inline-block;
    transform-origin: 50% 50%;
}

.spinning:hover {
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
