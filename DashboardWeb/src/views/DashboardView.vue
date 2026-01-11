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
            <component :is="currentView" />
            <SideBarComponent
                :sidebarViewsRight="sidebarViewsRight"
                side="right"
                @change-view="changeView"
                :currentView="currentView"
            />
        </div>
        <div class="course-tabs" v-if="currentView !== CalendarComponent && currentView !== ConstraintsComponent">
            <CourseTabsComponent @change-course="changeCourse" :currentCourse="currentCourse" :courses="courses"/>
        </div>
        <div class="todo-grid" v-if="currentView !== CalendarComponent && currentView !== ConstraintsComponent">
            <TodoGridComponent :currentCourse="currentCourse" :courses="courses"/>
        </div>
        <div class="scheduler-container" v-if="currentView === CalendarComponent">
            <SchedulerComponent />
        </div>
    </div>
</template>

<script setup>
import { ref, shallowRef, onMounted } from 'vue'
import ProfileComponent from '@/components/ProfileComponent.vue'
import SideBarComponent from '@/components/SideBarComponent.vue'
import CalendarComponent from '@/components/CalendarComponent.vue'
import CourseTabsComponent from '@/components/CourseTabsComponent.vue'
import TodoGridComponent from '@/components/TodoGridComponent.vue'
import SchedulerComponent from '@/components/SchedulerComponent.vue'
import ConstraintsComponent from '@/components/ConstraintsComponent.vue'
import api from '../services/api.js'

const isLoading = ref(false)
const currentView = shallowRef(CalendarComponent)
const currentCourse = ref(0)
const courses = ref([])

const sidebarViewsLeft = [
    { icon: 'ix:maintenance-octagon', height: 30, width: 30, tooltip: 'Maintenance', view: ProfileComponent },
    { icon: 'ep:calendar', height: 30, width: 30, tooltip: 'Calendar', view: CalendarComponent },
    { icon: 'uiw:dot-chart', height: 30, width: 30, tooltip: 'Constraints', view: ConstraintsComponent },
]
const sidebarViewsRight = [
    { icon: 'la:download', width: 30, height: 30, tooltip: 'Coming Soon', /*view: TodoGridComponent,*/ },
    { icon: 'icon-park-outline:redo', width: 30, height: 30, tooltip: 'Coming Soon', /*view: TodoGridComponent,*/ },
]

const fetchCourses = async () => {
    isLoading.value = true
    
    try {
        const response = await api.get('course')
        courses.value = response.data;
    } catch (error) {
        toast.error('Error fetching courses');
    } finally {
        isLoading.value = false
    }
}

const changeView = (view) => {
    if (view && view !== currentView.value) {
        currentView.value = view
    }
}

const changeCourse = (courseId) => {
    if (courseId && courseId !== currentCourse.value) {
        currentCourse.value = courseId
    }
}

onMounted(() => {
    fetchCourses()
})

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
    height: 45vh;
    width: 100%;
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
    width: 100%;
    height: 30vh;
    margin-top: 0.5rem;
    border-top: 2px solid $primary-text-light;
}
</style>
