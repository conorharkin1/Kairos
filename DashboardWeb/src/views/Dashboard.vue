<template>
    <div class="dashboard">
        <div class="current-view-container">
            <SideBar
                :sidebarViewsLeft="sidebarViewsLeft"
                side="left"
                @change-view="changeView"
                :currentView="currentView"
            />
            <component :is="currentView" />
            <SideBar
                :sidebarViewsRight="sidebarViewsRight"
                side="right"
                @change-view="changeView"
                :currentView="currentView"
            />
        </div>
        <div class="course-tabs">
            <CourseTabs />
        </div>
        <div class="todo-grid">
            <DashboardTodoGrid />
        </div>
    </div>
</template>

<script setup>
import { ref, shallowRef } from 'vue'
import ProfileView from '@/components/ProfileView.vue'
import SideBar from '@/components/SideBar.vue'
import CalendarView from '@/components/CalendarView.vue'
import CourseTabs from '@/components/CourseTabs.vue'
import DashboardTodoGrid from '@/components/DashboardTodoGrid.vue'

const currentView = shallowRef(ProfileView)

const sidebarViewsLeft = [
    {
        icon: 'iconoir:profile-circle',
        height: 30,
        width: 30,
        tooltip: 'Profile',
        view: ProfileView,
    },
    { icon: 'ep:calendar', height: 30, width: 30, tooltip: 'Calendar', view: CalendarView },
    {
        icon: 'uiw:dot-chart',
        height: 30,
        width: 30,
        tooltip: 'View Progress',
        view: DashboardTodoGrid,
    },
]
const sidebarViewsRight = [
    {
        icon: 'la:download',
        width: 30,
        height: 30,
        tooltip: 'Export Study Guide',
        view: DashboardTodoGrid,
    },
    {
        icon: 'icon-park-outline:redo',
        width: 30,
        height: 30,
        tooltip: 'Redo Study Guide',
        view: DashboardTodoGrid,
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
