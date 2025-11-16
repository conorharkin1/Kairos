<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-else-if="user" class="profile-container">
        <div class="profile-header">
            <h2>Hello, {{ user.firstName}}</h2>
        </div>
        <div class="profile-body">
            <div v-if="!user.isApiKeySet" class="key-input">
                <h3>It appears you haven't set your Canvas API key yet,</h3>
                <h3>Would you like to set it now?</h3>
                <div class="key-input">
                    <input type="text" placeholder="Enter your Canvas API key" v-model="apiKey" />
                    <button @click="saveApiKey" :disabled="!apiKey">Save API Key</button>
                </div>
            </div>
            <div v-else>
                <h4>You have already set your API key, would you like to update it?</h4>
                <div class="key-input">
                    <input type="text" placeholder="Enter a new API key" v-model="updatedApiKey" />
                    <button @click="updateApiKey" :disabled="!updatedApiKey">Update Key</button>
                </div>
            </div>
        </div>
        <div class="profile-footer">
            <button @click="logout" class="btn logout-btn">Logout</button>
            <button @click="handleDeleteAccount" class="btn delete-account-btn">Delete Account</button>
        </div>
    </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import api, { isLoading }from '@/services/api'
import { logout, deleteAccount } from '@/services/authService'
import { useToast } from "vue-toastification";

const apiKey = ref('')
const updatedApiKey = ref('')
const user = ref(null)
const toast = useToast()

const getCurrentUser = async () => {
    api.get('/user/self')
        .then(response => {
            user.value = response.data
        })
        .catch(error => {
            toast.error('Error while fetching user:', error)
        })
}

const saveApiKey = async () => {
    api.post('/user/key', apiKey.value ).then(response => {
            toast.success('API key saved successfully')
        })
        .catch(error => {
            toast.error('Error saving API key:', error)
        })
}

const updateApiKey = async () => {
    api.post('/user/key', updatedApiKey.value ).then(response => {
            toast.success('API key updated successfully')
            updatedApiKey.value = ''
        })
        .catch(error => {
            toast.error('Error updating API key:', error)
        })
        
}

const handleDeleteAccount = async () => {
    const confirmed = window.confirm('Are you sure you want to delete your account? This action cannot be undone.')
    if (confirmed) {
        try {
            await deleteAccount()
        } catch (error) {
            console.error('Error deleting account:', error)
            toast.error('Failed to delete account.')
        }
    }
}

onMounted(() => {
    getCurrentUser()
})
</script>

<style scoped lang="scss">
.profile-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    flex-grow: 1;
}

.profile-header {
    display: flex;
    justify-content: center;
    padding: 1rem;
    width: 80%;
    border-bottom: 1px solid $primary-red-outline;
    color: $primary-text-light;
}

.profile-body {
    display: flex;
    flex-direction: column;
    align-items: center;
    height: 100%;
    width: 80%;
    flex-grow: 1;
    color: $primary-text-light;
}

.profile-footer {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    width: 80%;
    margin-bottom: 2rem;
}

.profile-footer .btn {
    padding: 0.5rem;
    border: none;
    border-radius: 5px;
    width: 20%;
    font-weight: 600;
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

.loading-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-grow: 1;
}
.loader {
  width: fit-content;
  font-weight: bold;
  font-family: sans-serif;
  padding-bottom: 8px;
  background: linear-gradient(currentColor 0 0) 0 100%/0% 3px no-repeat;
  animation: l2 2s linear infinite;
  color: $primary-red-outline;
}
.loader:before {
  content:"Loading..."
}
</style>
