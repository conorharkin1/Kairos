<template>
    <div class="slider-component">
        <div class="slidecontainer">
            <input v-model.number="localValue" type="range" :min="min" :max="max" :step="step" class="slider"
                @input="handleInput" />
        </div>
    </div>
</template>

<script setup>
import { ref, watch } from 'vue';

const props = defineProps({
    modelValue: {
        type: Number,
        required: true
    },
    min: {
        type: Number,
        required: true
    },
    max: {
        type: Number,
        required: true
    },
    step: {
        type: Number,
        default: 1
    }
});

const emit = defineEmits(['update:modelValue', 'hasUnsavedChanges']);

const localValue = ref(props.modelValue);

watch(
    () => props.modelValue,
    (newValue) => {
        localValue.value = newValue;
    }
);

const handleInput = () => {
    emit('update:modelValue', localValue.value)
    emit('hasUnsavedChanges', true)
};
</script>


<style scoped lang="scss">
.slider-container {
    width: 100%;
}

/// ATTRIBUTION:
/// The below css classes were provided by Claude Sonnet, I then adapted them to fit the theme of the application
/// Prupose: To save time and effort researching very specific css for the slider component,
/// and to ensure the slider would appear correctly across all broswers.

.slider-component .slider {
    -webkit-appearance: none;
    appearance: none;
    width: 100%;
    height: 4px;
    border-radius: 2px;
    background: #c2c2c2;
    outline: none;
    opacity: 0.7;
    -webkit-transition: 0.2s;
    transition: opacity 0.2s;
}

.slider-component .slider:hover {
    opacity: 1;
}

.slider-component .slider::-webkit-slider-runnable-track {
    width: 100%;
    height: 8px;
    border-radius: 4px;
    background: #e0e0e0;
}

.slider-component .slider::-webkit-slider-thumb {
    -webkit-appearance: none;
    appearance: none;
    width: 20px;
    height: 20px;
    background: $primary-blue-colour;
    cursor: pointer;
    border-radius: 50%;
    margin-top: -6px;
    box-shadow: -400px 0 0 390px $primary-blue-colour;
    position: relative;
    z-index: 2;
}

.slider-component .slider::-moz-range-track {
    width: 100%;
    height: 8px;
    border-radius: 4px;
    background: #e0e0e0;
}

.slider-component .slider::-moz-range-progress {
    height: 8px;
    border-radius: 4px;
    background: $primary-blue-colour;
}

.slider-component .slider::-moz-range-thumb {
    width: 20px;
    height: 20px;
    background: $primary-blue-colour;
    cursor: pointer;
    border-radius: 50%;
    border: none;
    position: relative;
    z-index: 2;
}
</style>