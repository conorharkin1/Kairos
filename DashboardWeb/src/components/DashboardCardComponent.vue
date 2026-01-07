<template>
    <div class="card" :class="{ 'assignment': type === 'Assignment', 'quiz': type === 'Quiz' }">
        <div class="header card-type">
            <Icon class="btn-primary delete-btn" icon="fluent:delete-32-light" width="32" height="32" @click="emit('delete', id)"/>
            <h2>{{ type }}</h2>
            <Icon class="btn-primary edit-btn" icon="iconamoon:edit-thin" width="32" height="32" @click="toggleEditMode"/>
        </div>
        <div class="card-icon">
            <div v-if="type === 'Assignment'"><Icon icon="material-symbols-light:assignment-outline-rounded" width="32" height="32" /></div>
            <div v-else-if="type === 'Quiz'"><Icon icon="material-symbols-light:quiz-outline-rounded" width="32" height="32" /></div>
        </div>
        <div class="card-title">
            <h3 v-if="!isEditing">{{ model.name }}</h3>
            <input v-else v-model="updatedModel.name" class="input-field" placeholder="Name..."/>
        </div>
        <div class="card-due-date" v-if="type === 'Assignment'">
            <h2 v-if="!isEditing">Due on: {{ formattedDueDate }}</h2>
            <input v-else ref="dueDateInput" class="input-field" type="text" placeholder="Due date..." />
        </div>
        <div class="card-due-date" v-else-if="type === 'Quiz'">
            <h2 v-if="!isEditing">Due on: {{ formattedDueDate }}</h2>
            <input v-else ref="dueDateInput" class="input-field" type="text" placeholder="Due date..." />
            <h2 v-if="!isEditing">Locked on: {{ formattedLockDate }}</h2>
            <input v-else ref="lockDateInput" class="input-field" type="text" placeholder="Lock date..." />
        </div>
        <div class="card-body">
            <a v-if="!isEditing && model.htmlUrl" :href="model.htmlUrl" target="_blank">Go to {{ type }}</a>
            <h2 v-else-if="!isEditing">No URL available</h2>
            <input v-else v-model="updatedModel.htmlUrl" class="input-field" placeholder="URL..."/>
        </div>
        <div class="card-footer-checkboxes" v-if="!isEditing">
            <span v-if="model.isFavourite">Favourited: ✓</span>
            <span v-else>Not favourited</span>
            <span v-if="model.isComplete">Complete: ✓</span>
            <span v-else>Incomplete</span>
        </div>
        <div class="card-footer-checkboxes" v-else>
            <span>Is favourited?</span>
            <input type="checkbox" v-model="updatedModel.isFavourite">
            <span>Is complete?</span>
            <input type="checkbox" v-model="updatedModel.isComplete">
        </div>
        <div class="card-footer" v-if="isEditing">
            <button class="btn-primary btn-good" @click="emit('save', updatedModel)">Save</button>
        </div>
    </div>
</template>

<script setup>
import { ref, computed, watch, nextTick} from 'vue';
import { convertToUTC, convertToAppTimezone } from "@/utils/dayjsUtils";

const emit = defineEmits(['save', 'delete'])

const isEditing = ref(false)
const updatedModel = ref({})

const dueDateInput = ref(null);
const lockDateInput = ref(null);
let dueDateFp = null;
let lockDateFp = null;

const { model, type } = defineProps({
    model: Object,
    type: String
})

const formattedDueDate = computed(() =>
    model.dueDate ? convertToAppTimezone(model.dueDate).format('DD/MM/YYYY HH:mm') : 'No due date'
)
const formattedLockDate = computed(() =>
    model.lockDate ? convertToAppTimezone(model.lockDate).format('DD/MM/YYYY HH:mm') : 'No lock date'
)

function toggleEditMode() {
    isEditing.value = !isEditing.value
}

watch(isEditing, async (editing) => {
    if (!editing) {
        dueDateFp?.destroy();
        lockDateFp?.destroy();
        return;
    }

    updatedModel.value = { ...model };

    await nextTick();

    if (dueDateInput.value){
        dueDateFp = flatpickr(dueDateInput.value, {
            enableTime: true,
            dateFormat: "Y-m-d H:i",
            defaultDate: updatedModel.value.dueDate || null,
            onChange: (selectedDates, dateStr) => {
                updatedModel.value.dueDate = dateStr
            }
        });
    }
    
    if (lockDateInput.value && type === 'Quiz'){
        lockDateFp = flatpickr(lockDateInput.value, {
            enableTime: true,
            dateFormat: "Y-m-d H:i",
            defaultDate: updatedModel.value.lockDate || null,
            onChange: (selectedDates, dateStr) => {
                updatedModel.value.lockDate = dateStr
            }
        });
    }
});

</script>

<style scoped lang="scss">
.card {
    display: flex;
    flex-direction: column;
    max-width: 28rem;
    min-width: 28rem;
    min-height: 20rem;
    max-height: 450px;
    background-color: $primary-grey-background;
    color: $primary-text-light;
    height: 90%;
    border-radius: 10px;
}

.assignment {
    border-left: solid 4px $primary-red-colour;
}
.assignment:hover {
    box-shadow: 0 4px 8px 0 $primary-red-colour;
}
.quiz {
    border-left: solid 4px $primary-yellow-colour;
}
.quiz:hover {
    box-shadow: 0 4px 8px 0 $primary-yellow-colour;
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

.assignment .card-type h2 {
    color: $primary-red-colour;
    margin: 0.5rem 0;
}

.quiz .card-type h2 {
    color: $primary-yellow-colour;
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
    align-items: flex-start;
    padding: 0.5rem;
}

.card-footer-checkboxes {
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
