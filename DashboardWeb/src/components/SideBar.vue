<template>
    <div :class="side === 'left' ? 'side-bar-left' : 'side-bar-right'">
        <div class="side-bar-view" v-for="viewOption in side === 'left' ? sidebarViewsLeft : sidebarViewsRight" :key="viewOption.icon">
            <button v-tooltip="viewOption.tooltip" @click="$emit('change-view', viewOption.view)" :class="{ 'selected': currentView === viewOption.view }">
                <Icon :icon="viewOption.icon" :height="viewOption.height" :width="viewOption.width" />
            </button>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue';

const props = defineProps({
    sidebarViewsLeft: Array,
    sidebarViewsRight: Array,
    side: String,
    currentView: Object
});

const emit = defineEmits(['change-view']);

</script>

<style scoped lang="scss">
    .profile-widget {
        color: $primary-red-outline;
    }
    .side-bar-left{
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 3%;
        background-color: $primary-background-dark;
        margin-right: 1rem;
    }
    .side-bar-right{
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 3%;
        background-color: $primary-background-dark;
        margin-left: 1rem;
    }
    .side-bar-view {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        width: 100%;
        padding-bottom: 1rem;
        margin-bottom: 2rem;
        color: $primary-text-light;
    }
    .side-bar-view button {
        background: none;
        border: none;
        cursor: pointer;
        color: $primary-text-light;
        font-size: 1rem;
        position: relative;
    }
    .side-bar-view button:hover {
        color: $primary-red-outline;
    }
    .side-bar-view button.selected {
        color: $primary-red-outline;
    }
</style>