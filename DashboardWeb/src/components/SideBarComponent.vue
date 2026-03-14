<template>
    <div :class="side === 'left' ? 'side-bar-left' : 'side-bar-right'">
        <div class="side-bar-view" v-for="viewOption in side === 'left' ? sidebarViewsLeft : sidebarViewsRight"
            :key="viewOption.icon">
            <button v-if="!viewOption.action" :class="{ selected: currentView === viewOption.view }" v-tooltip="viewOption.tooltip"
                @click="$emit('change-view', viewOption.view)">
                <Icon :icon="viewOption.icon" :height="viewOption.height" :width="viewOption.width" />
            </button>
            <button v-else v-tooltip="viewOption.tooltip" @click="$emit('handle-action', viewOption.action)">
                <Icon :icon="viewOption.icon" :height="viewOption.height" :width="viewOption.width" />
            </button>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue'

const props = defineProps({
    sidebarViewsLeft: Array,
    sidebarViewsRight: Array,
    side: String,
    currentView: Object,
    action: String,
})

const emit = defineEmits(['change-view', 'handle-action'])
</script>

<style scoped lang="scss">
.profile-widget {
    color: $primary-red-colour;
}

.side-bar-left {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 3%;
    background-color: $primary-dark-background;
    margin-right: 1rem;
}

.side-bar-right {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 3%;
    background-color: $primary-dark-background;
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
    color: $primary-red-colour;
}

.side-bar-view button.selected {
    color: $primary-red-colour;
}
</style>
