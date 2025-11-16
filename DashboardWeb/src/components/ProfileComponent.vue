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
            <button @click="logout" class="logout-btn">Logout</button>
            <button @click="handleDeleteAccount" class="delete-account-btn">Delete Account</button>
        </div>
    </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import api from '@/services/api'
import { logout, deleteAccount } from '@/services/authService'

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

const handleDeleteAccount = async () => {
    const confirmed = window.confirm('Are you sure you want to delete your account? This action cannot be undone.')
    if (confirmed) {
        try {
            await deleteAccount()
        } catch (error) {
            console.error('Error deleting account:', error)
            alert('Failed to delete account.')
        }
    }
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

.logout-btn {
    cursor: pointer;
}
.delete-account-btn {
    background-color: $primary-red-outline;
    cursor: pointer;
}
</style>
