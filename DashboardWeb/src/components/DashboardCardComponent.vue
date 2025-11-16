<template>
    <div class="card">
        <div class="card-type">
            <h2>{{ type }}</h2>
        </div>
        <div class="card-title">
            <h3>{{ title }}</h3>
        </div>
        <div v-if="dueDate" class="card-due-date">
            <span class="due-date">Due: {{ dueDate }}</span>
        </div>
        <div class="card-body">
            <p>{{ description }}</p>
        </div>
        <div class="card-footer">
            <button class="card-btn">Favourite</button>
            <button class="card-btn">Mark as Complete</button>
        </div>
    </div>
</template>

<script setup>

const props = defineProps({
    type: String,
    title: String,
    dueDate: String,
    description: String
})
</script>

<style scoped lang="scss">
.card {
    display: flex;
    flex-direction: column;
    max-width: 25rem;
    min-width: 20rem;
    border: solid 2px $primary-text-light;
    color: $primary-text-light;
    height: 90%;
    border-radius: 10px;
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
</style>
