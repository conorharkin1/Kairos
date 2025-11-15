<template>
    <div class="profile-container">
        <div class="profile">
            <h2>Hello! Conor Harkin</h2>
            <h3>It appears you haven't set your Canvas API key yet,</h3>
            <h3>Would you like to set it now?</h3>
            <div class="key-input">
                <input type="text" placeholder="Enter your Canvas API key" v-model="apiKey" />
                <button @click="saveApiKey" :disabled="!apiKey">Save API Key</button>
            </div>
        </div>
    </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import api from '@/services/api'

const apiKey = ref('')

const saveApiKey = async () => {
    console.log(apiKey.value)
    api.post('/user/key', apiKey.value ).then(response => {
            console.log('API key saved successfully')
        })
        .catch(error => {
            console.error('Error saving API key:', error)
        })
        
}

onMounted(() => {
    // Any initialization logic can go here
})
</script>

<style scoped lang="scss">
.profile-container {
    flex-grow: 1;
}

.profile {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    color: $primary-text-light;
}

.key-input {
    display: flex;
    flex-direction: row;
    color: $primary-text-light;
    input {
        height: 100%;
        width: 100%;
        padding: 0.5rem;
    }

    button {
        margin-left: 0.5rem;
    }
}
</style>
