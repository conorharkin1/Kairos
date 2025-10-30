<template>
    <div class="side-bar">
        <div class="side-bar-option">
            <button>
                <Icon icon="iconoir:profile-circle" height="30" width="30" />
            </button>
        </div>
        <div class="side-bar-option">
            <button>
                <Icon icon="ep:calendar" height="30" width="30" />
            </button>
        </div>
        <div class="side-bar-option">
            <button>
                <Icon icon="uiw:dot-chart" width="30" height="30" />
            </button>
        </div>
    </div>
</template>

<style scoped lang="scss">
    .profile-widget {
        color: $primary-red-outline;
    }
    .side-bar {
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 3%;
        background-color: $primary-background-dark;
        margin-right: 1rem;
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
    }
    .side-bar-option button:hover {
        color: $primary-red-outline;
    }
</style>