<template>
    <div :class="side === 'left' ? 'side-bar-left' : 'side-bar-right'">
        <div class="side-bar-option " v-for="option in side === 'left' ? sidebarOptionsLeft : sidebarOptionsRight" :key="option.icon">
            <button v-tooltip="option.tooltip">
                <Icon :icon="option.icon" :height="option.height" :width="option.width" />
            </button>
        </div>
    </div>
</template>

<script setup>
import { ref } from 'vue';

const props = defineProps({
  sidebarOptionsLeft: {
    type: Array,
    default: () => ([])
  },
  sidebarOptionsRight: {
    type: Array,
    default: () => ([])
  },
  side: String,
});

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
    .side-bar-option {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        width: 100%;
        padding-bottom: 1rem;
        margin-bottom: 2rem;
        color: $primary-text-light;
    }
    .side-bar-option button {
        background: none;
        border: none;
        cursor: pointer;
        color: $primary-text-light;
        font-size: 1rem;
        position: relative;
    }
    .side-bar-option button:hover {
        color: $primary-red-outline;
    }
</style>