<template>
    <div class="drag-container drag-wrapper" v-drag-and-drop:options="dndOptions">
        <b-form-input v-model="search" placeholder="Search tasks"></b-form-input>
        
        <!-- Render all the base level tasks -->
        <vue-draggable-group v-model="tasks" :groups="tasks">
            <div class="tasks" >
                <task v-for="task in mainTasks" :key="task.id" v-bind:task="task" :tasks="tasks" />
            </div>
        </vue-draggable-group>
        
        
    </div>
</template>
<script lang="js">
    import Vue from 'vue';
    //import axios from 'axios';
    import Task from './Task.vue'
    import { VueDraggableDirective } from 'vue-draggable'

    export default Vue.extend({
        directives: {
            dragAndDrop: VueDraggableDirective
        },
        components: {
            Task
        },
        data() {
            return {
                dndOptions: {
                    dropzoneSelector: '.tasks',
                    draggableSelector: '.task',
                    onDragend: (e,d) => { console.log(e); console.log(d); },

                },
                loading: false,
                tasks: [
                    {
                        id: 1,
                        summary: 'Go to store',
                        description: 'Purchase milk and bread',
                        createdAt: "2021-09-03T18:02:20",
                        deadlineAt: "2021-09-21T12:00:00",
                        priority: 1,
                        status: 0,
                        parentId: null
                    },
                    {
                        id: 2,
                        summary: 'Purchase lederhosen',
                        description: 'It\'s almost oktoberfest season',
                        createdAt: "2021-09-03T18:02:20",
                        deadlineAt: null,
                        priority: 1,
                        status: 0,
                        parentId: null,
                    },
                    {
                        id: 3,
                        summary: 'Purchase lederhosen 2',
                        description: 'It\'s almost oktoberfest season',
                        createdAt: "2021-09-03T18:02:20",
                        deadlineAt: null,
                        priority: 1,
                        status: 0,
                        parentId: 2
                    },
                    {
                        id: 4,
                        summary: 'Purchase lederhosen 3',
                        description: 'It\'s almost oktoberfest season',
                        createdAt: "2021-09-03T18:02:20",
                        deadlineAt: null,
                        priority: 1,
                        status: 0,
                        parentId: 2
                    }
                ],
                search: ''
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData() {
                this.post = null;
                this.loading = true;
                /*axios.get("/api/tasks").then(d => {
                    console.log(d.data);
                })*/
            },
            onTaskUpdated(task) {
                let i = this.tasks.findIndex(t => t.id === task.id);
                this.tasks[i] = task;
            },
            onDragend(e) {
                console.log(e)
            }
        },
        computed: {
            // Tasks that have no parent
            mainTasks() {
                return this.tasks.filter(t => { return !t.parentId })
            }
        }
    });
</script>