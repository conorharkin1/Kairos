<template>
    <div v-if="isLoading" class="loading-container loader"></div>
    <div v-else class="dashboard">
        <div class="current-view-container" :class="{'current-view-container-calendar': currentView === CalendarComponent}">
            <SideBarComponent
                :sidebarViewsLeft="sidebarViewsLeft"
                side="left"
                @change-view="changeView"
                :currentView="currentView"
            />
            <component :is="currentView" :courses="courses" ref="currentViewRef"/>
            <SideBarComponent
                :sidebarViewsRight="sidebarViewsRight"
                side="right"
                @change-view="changeView"
                @handle-action="handleAction"
                :currentView="currentView"
            />
        </div>
        <div class="course-tabs" v-if="currentView !== CalendarComponent && currentView !== ConstraintsComponent">
            <CourseTabsComponent @set-current-course="setCurrentSelectedCourse" @set-courses="courses = $event"/>
        </div>
        <div class="todo-grid" v-if="currentView !== CalendarComponent && currentView !== ConstraintsComponent">
            <TodoGridComponent :currentCourse="currentCourse" :courses="courses"/>
        </div>
        <div class="scheduler-container" v-if="currentView === CalendarComponent">
            <SchedulerComponent @schedule-created="onScheduleCreated" />
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
import SchedulerComponent from '@/components/SchedulerComponent.vue'
import ConstraintsComponent from '@/components/ConstraintsComponent.vue'
import api from '../services/api.js'
import DisplayErrorMessage from '@/services/error'
import { useToast } from 'vue-toastification'

const toast = useToast()

const isLoading = ref(false)
const currentView = shallowRef(CalendarComponent)
const currentCourse = ref(0)
const courses = ref([])
const currentViewRef = ref(null)

const onScheduleCreated = () => {
    if (currentViewRef.value?.refreshSchedule) {
        currentViewRef.value.refreshSchedule()
    }
}

const sidebarViewsLeft = [
    { icon: 'ix:maintenance-octagon', height: 30, width: 30, tooltip: 'Profile and Dashboard', view: ProfileComponent },
    { icon: 'ep:calendar', height: 30, width: 30, tooltip: 'Calendar', view: CalendarComponent },
    { icon: 'uiw:dot-chart', height: 30, width: 30, tooltip: 'Constraints', view: ConstraintsComponent },
]
const sidebarViewsRight = [
    { icon: 'la:download', width: 30, height: 30, tooltip: 'Export Schedule', action: 'export' },
]

const changeView = (view) => {
    if (view && view !== currentView.value) {
        currentView.value = view
    }
}

/// ATTRIBUTION:
/// The below code for downloading a file was adapted from a solution on StackOverflow
/// Source: https://stackoverflow.com/questions/53772331/vue-html-js-how-to-download-a-file-to-browser-using-the-download-tag
const handleAction = async (action) => {
    if (action === 'export') {
        try {
            isLoading.value = true;
            const response = await api.get('scheduler/export', { responseType: 'blob' });
            const blob = await response.data;

            const url = window.URL.createObjectURL(blob);
            const link = document.createElement("a");

            link.href = url;
            link.download = `weeklyschedule-${new Date().toISOString()}.ics`;
            link.click();

            window.URL.revokeObjectURL(url);
        } catch (error) {
            if (!DisplayErrorMessage(error)) {
                toast.error('An error occurred while exporting your schedule. Please try again.')
            }
        } finally {
            isLoading.value = false;
        }
    }
}

const setCurrentSelectedCourse = (courseId) => {
    if (courseId && courseId !== currentCourse.value) {
        currentCourse.value = courseId
    }
}

</script>

<style scoped lang="scss">
.dashboard {
    display: flex;
    flex-direction: column;
    background-color: $primary-dark-background;
}
.current-view-container {
    display: flex;
    flex-direction: row;
    min-height: 45vh;
    width: 100%;
    max-width: 100%;
    overflow-x: hidden;
    box-sizing: border-box;
}
.current-view-container-calendar {
    height: 65vh;
    margin-bottom: 0.5rem;
}
.course-tabs {
    display: flex;
    overflow: hidden;
    flex-direction: row;
    align-items: center;
    width: 100%;
    color: $primary-text-light;
    border: 2px solid $primary-text-light;
    text-wrap: nowrap;
    height: 5vh;
}
.todo-grid {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 45vh;
    margin-top: 0.5rem;
}
.scheduler-container {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    width: 100%;
    height: 30vh;
    margin-top: 0.5rem;
    border-top: 2px solid $primary-text-light;
}
</style>
