<template>
    <div class="card">
        <div class="header card-type">
            <Icon class="btn-primary delete-btn" icon="fluent:delete-32-light" width="32" height="32" @click="emit('delete')"/>
            <h2>{{ type }}</h2>
            <Icon class="btn-primary edit-btn" icon="iconamoon:edit-thin" width="32" height="32" @click="toggleEditMode"/>
        </div>
        <div class="card-title">
            <div v-if="type === 'Assignment'"><Icon icon="material-symbols-light:assignment-outline-rounded" width="32" height="32" /></div>
        </div>
        <div class="card-title">
            <h3 v-if="!isEditing">{{ title }}</h3>
            <input class="input-field" v-else v-model="updatedData.title" placeholder="Title..."/>
        </div>
        <div v-if="dueDate" class="card-due-date">
            <span class="due-date">Due: {{ dueDate }}</span>
        </div>
        <div class="card-body">
            <a :href="htmlUrl" target="_blank">Link to {{ type }}</a>
        </div>
        <div class="card-footer" v-if="!isEditing">
            <button class="card-btn">Favourite</button>
            <button class="card-btn">Mark as Complete</button>
        </div>
        <div class="card-footer" v-else>
            <button class="card-btn" @click="emit('save', updatedData)">Save</button>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue'

const isEditing = ref(false)
const updatedData = ref({})

const props = defineProps({
    type: String,
    title: String,
    dueDate: String,
    description: String,
    htmlUrl: String,
})

function toggleEditMode() {
    isEditing.value = !isEditing.value
}

const emit = defineEmits(['save', 'delete'])
</script>

<style scoped lang="scss">
.card {
    display: flex;
    flex-direction: column;
    max-width: 28rem;
    min-width: 28rem;
    min-height: 20rem;
    background-color: $primary-grey-background;
    color: $primary-text-light;
    height: 90%;
    border-radius: 10px;
    // box-shadow: 0 4px 8px 0 $primary-red-colour;
    border-left: solid 4px $primary-red-colour;
}

.card:hover {
    box-shadow: 0 4px 8px 0 $primary-red-colour;
}

.header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0.5rem;
}

.card-type,
.card-title,
.card-due-date,
.card-body {
    text-align: center;
}
.card-body {
    flex-grow: 1;
}
.card-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0.5rem;
}

.edit-btn:hover {
    color: $primary-blue-colour;
}

.delete-btn:hover {
    color: $primary-red-colour;
}

.input-field {
    background-color: $primary-dark-background;
    color: $primary-text-light;
    border: 2px solid $primary-text-light;
    border-radius: 4px;
    font-size: 1rem;
    outline: none;
    padding: 0.5rem;
    width: 80%;
}

</style>
