<template>
    <div>
        <b-form-input v-model="search" placeholder="Search tasks"></b-form-input>
        <b-form-select v-model="sorting" :options="config.sortingOptions"></b-form-select>
        
        <!-- Render all the base level tasks -->
        <draggable :list="computedTasks" :group="{ name: 'tasks' }" handle=".drag-handle" ghost-class="ghost">
            <task v-for="task in computedTasks" :key="task.id" v-bind:task="task" :tasks="tasks" @save="save" @remove="remove" />
        </draggable>
        
        <b-button v-b-modal.new-task>New task</b-button>
        <task-editor id="new-task" :autosave="false" :ismodal="true" @save="save"></task-editor>
    </div>
</template>
<script lang="js">
    import Vue from 'vue';
    import axios from 'axios';
    import task from './Task.vue';
    import taskEditor from './TaskEditor.vue';
    import draggable from 'vuedraggable';
    import sorting from '../config/sorting.js'

    export default Vue.extend({
        components: {
            task,
            draggable,
            taskEditor
        },
        data() {
            return {
                config: {
                    sortingOptions: sorting.options
                },
                loading: false,
                tasks: [],
                sorting: {},
                search: ''
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        computed: {
            computedTasks() {
                let tasks = this.tasks;
                if (this.sorting) {
                    tasks.sort((a,b) => { 
                        if (a[this.sorting.field] > b[this.sorting.field]) {
                            return this.sorting.ascending ? 1 : -1;
                        }
                        if (a[this.sorting.field] < b[this.sorting.field]) {
                            return this.sorting.ascending ? -1 : 1;
                        }
                        return 0;
                     });    
                }
                console.log(this.search);
                if (this.search) {
                    tasks = tasks.filter(t => t.summary.toLowerCase().includes(this.search.toLowerCase()));
                }
                return tasks;
            }
        },
        methods: {
            fetchData() {
                this.post = null;
                this.loading = true;
                axios.get('/api/tasks').then(r => {
                    console.log(r);
                    this.tasks = r.data;
                })
            },
            save(task, savedCallback) { 
                // make a copy of the data and save changes
                let data = JSON.parse(JSON.stringify(task));
                data.children = null;
                
                if ( data.id ) {
                    axios.put(`/api/tasks/${data.id}`, data).then(r => {
                        console.log(r);
                    });
                }
                else {
                    axios.post('/api/tasks', data).then(r => {
                        if (savedCallback) {
                            savedCallback(r.data);
                        }
                        else {
                            this.tasks.push(r.data);
                        }
                    });
                }
            },
            remove(task, removedCallback) {
                axios.delete(`/api/tasks/${task.id}`).then(() => {
                    if (removedCallback) {
                        removedCallback(task);
                    }
                    else {
                        let i = this.tasks.findIndex(c => c.id == task.id);
                        this.$delete(this.tasks, i);
                        console.log('remove from root');
                    }
                });
            }
        },
    });
</script>
<style scoped>
    .ghost {
        background: #000;
    }
</style>