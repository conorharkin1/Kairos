<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-else class="content">
        <div>
            <h2>Organise your thoughts</h2>
        </div>
        <div class="wrapper">
            <form @submit="registerUser">
                <h1>Create a new account</h1>
                <div class="input-box">
                    <input type="text" v-model="firstname" required placeholder="Forename" />
                </div>
                <div class="input-box">
                    <input type="text" v-model="lastname" required placeholder="Surname" />
                </div>
                <div class="input-box">
                    <input type="email" v-model="email" required placeholder="Email" />
                </div>
                <div class="input-box">
                    <input type="password" v-model="password" required placeholder="Password" />
                </div>
                <div class="input-box">
                    <input
                        type="password"
                        v-model="confirmPassword"
                        required
                        placeholder="Confirm password"
                    />
                </div>
                <button class="btn" type="submit">Register</button>
                <div class="login-link">
                    <p>
                        <a @click="goToLogin">Already have an account? Login</a>
                    </p>
                </div>
            </form>
        </div>
    </div>
    
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { register } from '@/services/authService'
import DisplayErrorMessage from '@/services/error'
import { useToast } from 'vue-toastification'
const toast = useToast()

const email = ref('')
const password = ref('')
const confirmPassword = ref('')
const firstname = ref('')
const lastname = ref('')
const router = useRouter()
const isLoading = ref(false)

const registerUser = async (event) => {
    isLoading.value = true
    event.preventDefault()

    if (password.value !== confirmPassword.value) {
        toast.error('Passwords do not match!')
        isLoading.value = false
        return
    }

    try {
        await register({
            email: email.value,
            password: password.value,
            firstname: firstname.value,
            lastname: lastname.value,
        })
        router.push('/login')
    } catch (error) {
        console.log('Registration error:', error)
        if (!DisplayErrorMessage(error)) {
            toast.error('An error occurred while registering. Please try again.')
        }
    } finally {
        isLoading.value = false
    }
}
const goToLogin = () => {
    router.push('/login')
}
</script>

<style scoped lang="scss">
.content {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background: $primary-dark-background;
}

.content h2 {
    padding-right: 2rem;
    color: $primary-text-light;
    font-size: 30px;
}

.wrapper {
    width: 420px;
    color: $primary-text-light;
    border-radius: 10px;
    padding: 30px 40px;
    display: flex;
    justify-content: center;
}

.wrapper h1 {
    font-size: 36px;
    text-align: center;
}

.wrapper .input-box {
    width: 100%;
    height: 50px;
    margin: 30px 0;
    display: flex;
    justify-content: center;
    align-items: center;
}

.input-box input {
    width: 100%;
    height: 80%;
    background: transparent;
    border: 2px solid $primary-red-colour;
    border-radius: 20px;
    outline: none;
    color: $primary-text-light;
    padding-left: 1rem;
}

.wrapper .btn {
    width: 100%;
    height: 45px;
    background: $primary-red-colour;
    border: none;
    outline: none;
    border-radius: 20px;
    cursor: pointer;
    color: $primary-text-light;
    font-weight: 600;
}

.wrapper .login-link {
    font-size: 14px;
    text-align: center;
    margin: 20px 0 15px;
}

.login-link p a {
    color: $primary-text-light;
    text-decoration: none;
    font-weight: 600;
    font-size: 16px;
}

.login-link p a:hover {
    text-decoration: underline;
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
@keyframes l2 {to{background-size: 100% 3px}}

</style>
