<template>
    <div class="slider-component">
        <div class="slidecontainer">
            <input v-model.number="localValue" type="range" :min="min" :max="max" :step="step"
                class="slider" @input="handleInput" />
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


<style scoped>
.slider-component .slidecontainer {
    width: 100%;
}

.slider-component .slidecontainer .slider {
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

.slider-component .slidecontainer .slider:hover {
    opacity: 1;
}

.slider-component .slidecontainer .slider::-webkit-slider-thumb {
    -webkit-appearance: none;
    appearance: none;
    width: 18px;
    height: 18px;
    background: #d8a22e;
    cursor: pointer;
    border-radius: 50%;
}

.slider-component .slidecontainer .slider::-moz-range-thumb {
    width: 18px;
    height: 18px;
    background: #d8a22e;
    cursor: pointer;
    border-radius: 50%;
}
</style>