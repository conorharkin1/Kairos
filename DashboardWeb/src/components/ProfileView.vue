<template>
  <div class="profile-container">
    <div class="profile">
      <h2>Conor Harkin</h2>
      <div class="key-input">
        <input type="text" placeholder="Enter your Canvas API key" v-model="apiKey" />
        <button @click="saveApiKey" :disabled="!apiKey">Save API Key</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import apiClient from '@/services/api'

const apiKey = ref('')

const saveApiKey = async () => {
  console.log(apiKey.value)
  const response = await apiClient.post('api/profile/key', { key: apiKey.value })

  if (response.status === 200) {
    console.log('API key set successfully')
  } else {
    console.error('Failed to set API key')
  }
}
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
