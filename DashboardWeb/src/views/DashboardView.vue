<template>
    <div class="dashboard">
        <div class="current-view-container">
            <SideBarComponent
                :sidebarViewsLeft="sidebarViewsLeft"
                side="left"
                @change-view="changeView"
                :currentView="currentView"
            />
            <component :is="currentView" />
            <SideBarComponent
                :sidebarViewsRight="sidebarViewsRight"
                side="right"
                @change-view="changeView"
                :currentView="currentView"
            />
        </div>
        <div class="course-tabs">
            <CourseTabsComponent />
        </div>
        <div class="todo-grid">
            <TodoGridComponent />
        </div>
    </div>
</template>

<script setup>
import { ref, shallowRef } from 'vue'
import ProfileComponent from '@/components/ProfileComponent.vue'
import SideBarComponent from '@/components/SideBarComponent.vue'
import CalendarComponent from '@/components/CalendarComponent.vue'
import CourseTabsComponent from '@/components/CourseTabsComponent.vue'
import TodoGridComponent from '@/components/TodoGridComponent.vue'

const currentView = shallowRef(ProfileComponent)

const sidebarViewsLeft = [
    {
        icon: 'iconoir:profile-circle',
        height: 30,
        width: 30,
        tooltip: 'Profile',
        view: ProfileComponent,
    },
    { icon: 'ep:calendar', height: 30, width: 30, tooltip: 'Calendar', view: CalendarComponent },
    {
        icon: 'uiw:dot-chart',
        height: 30,
        width: 30,
        tooltip: 'View Progress',
        view: TodoGridComponent,
    },
]
const sidebarViewsRight = [
    {
        icon: 'la:download',
        width: 30,
        height: 30,
        tooltip: 'Export Study Guide',
        view: TodoGridComponent,
    },
    {
        icon: 'icon-park-outline:redo',
        width: 30,
        height: 30,
        tooltip: 'Redo Study Guide',
        view: TodoGridComponent,
    },
]

const changeView = (view) => {
    if (view && view !== currentView.value) {
        currentView.value = view
    }
}
</script>

<style scoped lang="scss">
.dashboard {
    display: flex;
    flex-direction: column;
    background-color: $primary-background-dark;
}
.current-view-container {
    display: flex;
    flex-direction: row;
    height: 45vh;
    width: 100%;
}
.course-tabs {
    display: flex;
    overflow-x: auto;
    flex-direction: row;
    align-items: center;
    width: 100%;
    color: $primary-text-light;
    border: 2px solid $primary-text-light;
    text-wrap: nowrap;
    height: 4vh;
}
.todo-grid {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 45vh;
    margin-top: 0.5rem;
}
</style>
