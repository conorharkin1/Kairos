<template>
    <div class="card">
        <div class="header card-type">
            <Icon class="btn-primary delete-btn" icon="fluent:delete-32-light" width="32" height="32" @click="emit('delete', id)"/>
            <h2>{{ type }}</h2>
            <Icon class="btn-primary edit-btn" icon="iconamoon:edit-thin" width="32" height="32" @click="toggleEditMode"/>
        </div>
        <div class="card-icon">
            <div v-if="type === 'Assignment'"><Icon icon="material-symbols-light:assignment-outline-rounded" width="32" height="32" /></div>
        </div>
        <div class="card-title">
            <h3 v-if="!isEditing">{{ title }}</h3>
            <input v-else v-model="updatedData.title" class="input-field" placeholder="Title..."/>
        </div>
        <div class="card-due-date">
            <h2 v-if="!isEditing">Due on: {{ formattedDueDate }}</h2>
            <input v-else ref="dueDateInput" class="input-field" v-model="updatedData.dueDate" type="text" placeholder="Due date..." />
        </div>
        <div class="card-body">
            <a v-if="!isEditing" :href="htmlUrl" target="_blank">Go to {{ type }}</a>
            <input v-else v-model="updatedData.htmlUrl" class="input-field" placeholder="URL..."/>
        </div>
        <div class="card-footer" v-if="!isEditing">
            <button class="btn-primary btn-grey card-btn">Favourite</button>
            <button class="btn-primary btn-grey card-btn">Mark as Complete</button>
        </div>
        <div class="card-footer footer-editing" v-else>
            <button class="btn-primary btn-good" @click="emit('save', updatedData)">Save</button>
        </div>
    </div>
</template>

<script setup>
import { ref, computed, watch, nextTick } from 'vue'
import { convertToUTC, convertToAppTimezone } from "@/utils/dayjsUtils";

const isEditing = ref(false)
const updatedData = ref({})

const dueDateInput = ref(null);
let fp = null;

const props = defineProps({
    type: String,
    title: String,
    dueDate: String,
    description: String,
    htmlUrl: String,
})

const emit = defineEmits(['save', 'delete'])

const formattedDueDate = computed(() =>
    props.dueDate ? convertToAppTimezone(props.dueDate).format('DD/MM/YYYY HH:mm') : 'No due date'
)

function toggleEditMode() {
    isEditing.value = !isEditing.value
}

watch(isEditing, async (editing) => {
    if (!editing) {
        fp?.destroy();
        return;
    }

    await nextTick();
    fp = flatpickr(dueDateInput.value, {
        enableTime: true,
        dateFormat: "Y-m-d H:i",
    });
});

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
.card-icon,
.card-due-date,
.card-body {
    text-align: center;
}

.card-title {
    min-height: 20%;
    margin-top: 1rem;
}

.card-type h2 {
    color: $primary-red-colour;
    margin: 0.5rem 0;
}

.card-icon {
    border-bottom: solid 1px $primary-text-light;
}

.card-body {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-grow: 1;
}

.card-body a {
    color: $primary-text-light;
    text-decoration: none;
    font-size: larger;
}
.card-body a:hover {
    text-decoration: underline;
}
.card-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0.5rem;
}

.card-footer button {
    width: 40%;
}

.footer-editing {
    display: flex;
    justify-content: flex-end;
}

.edit-btn {
    width: 10%;
}

.delete-btn {
    width: 10%;
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

.card-btn:hover {
    background-color: $primary-text-light;
    color: $primary-text-dark;
}

Icon {
    cursor: pointer;
}

</style>
