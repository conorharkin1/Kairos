<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-else-if="user" class="profile-container">
        <div class="profile-header">
            <h2>Hello, {{ user.firstName}}</h2>
        </div>
        <div class="profile-body">
            <div v-if="!user.isApiKeySet">
                <h3>It appears you haven't set your Canvas API key yet, would you like to set it now?</h3>
                <div class="key-input">
                    <input type="text" placeholder="Enter your Canvas API key" v-model="apiKey" />
                    <button class="btn-primary" @click="saveApiKey" :disabled="!apiKey" :class="{ 'btn-good': apiKey }">Save API Key</button>
                </div>
            </div>
            <div v-else>
                <h4>You have already set your API key, would you like to update it?</h4>
                <div class="key-input">
                    <input type="text" placeholder="Enter a new API key" v-model="updatedApiKey" />
                    <button class="btn-primary" @click="updateApiKey" :disabled="!updatedApiKey" :class="{ 'btn-good': updatedApiKey }">Update Key</button>
                </div>
            </div>
            <h4 v-if="user.isApiKeySet">Last refreshed data: {{ formattedLastUpdated }}.</h4>
        </div>
        <div class="profile-footer">
            <button @click="logout" class="btn-primary btn-grey">Logout</button>
            <button @click="updateDataFromCanvas" class="btn-primary btn-good" v-tooltip="'This will check the systems records against your Canvas and keep everything up to date'">Refresh Data from Canvas</button>
            <button @click="handleDeleteAccount" class="btn-primary btn-danger">Delete Account</button>
        </div>
    </div>
</template>

<script setup>
import { computed, onMounted, ref } from 'vue'
import api from '@/services/api'
import { logout, deleteAccount } from '@/services/authService'
import { useToast } from "vue-toastification";
import { convertToUTC, convertToAppTimezone } from "@/utils/dayjsUtils";
import DisplayErrorMessage from '@/services/error'

const apiKey = ref('')
const updatedApiKey = ref('')
const user = ref(null)
const toast = useToast()
const isLoading = ref(false)

const getCurrentUser = async () => {
    isLoading.value = true

    try {
        const response = await api.get('/user/self');
        user.value = response.data
    } catch (error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while fetching profile. Logging out')
        }
        logout()
    } finally {
        isLoading.value = false
    }
}

const saveApiKey = async () => {
    isLoading.value = true

    try {
        await api.post('/user/key', apiKey.value);
        getCurrentUser()
        toast.success('API key saved successfully')
    } catch(error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('Error saving API key')
        }
    } finally {
        isLoading.value = false
    }
}

const updateApiKey = async () => {
    isLoading.value = true

    try {
        await api.post('/user/key', updatedApiKey.value);
        toast.success('API key updated successfully')
    } catch (error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('Error saving API key')
        }
    } finally {
        isLoading.value = false
    }
}

const handleDeleteAccount = async () => {
    isLoading.value = true

    const confirmed = window.confirm('Are you sure you want to delete your account? This action cannot be undone.')
    if (confirmed) {
        try {
            await deleteAccount()
        } catch (error) {
            if (!DisplayErrorMessage(error)) {
                toast.error('An error occurred while deleting your account. Please try again.')
            }
        } finally {
            isLoading.value = false
        }
    }
}

const updateDataFromCanvas = async () => {
    isLoading.value = true
    
    toast.info('Updating data from Canvas. This may take a few moments...')
    try {
        await api.post('/user/refresh/canvas')
        await getCurrentUser()
        toast.success('Data updated successfully.')
        toast.warning('Please refresh the page for changes to take effect.')
    } catch (error) {
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while fetching data from Canvas. Please try again later.')
        }
    } finally {
        isLoading.value = false
    }
}

const formattedLastUpdated = computed(() =>
    user.value?.lastUpdatedData ? convertToAppTimezone(user.value.lastUpdatedData).format('DD/MM/YYYY HH:mm') : 'Never refreshed'
)

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
    border-bottom: 1px solid $primary-red-colour;
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

.profile-body h4 {
    margin-top: 2rem;
}

.profile-footer {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    width: 80%;
    margin-bottom: 2rem;
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
        width: 30%;
    }
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
  color: $primary-red-colour;
}
.loader:before {
  content:"Loading..."
}
</style>
