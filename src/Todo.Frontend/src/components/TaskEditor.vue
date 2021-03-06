<template>
    <component :is="component" :id="id" :title="title" @ok="save">
        <b-form @submit="onChange">
            <b-form-group label="Summary" label-align="left" label-class="font-weight-bold" :label-for="'input-'+task.id+'-summary'">
                <b-form-input @change="onChange" :id="'input-'+task.id+'-summary'" v-model="task.summary" :state="summaryState" type="text"></b-form-input>
                <b-form-invalid-feedback>
                    <template v-if="task.summary && task.summary.length > config.validation.maxSummary">
                        Max summary length is {{ config.validation.maxSummary }} characters.
                    </template>
                </b-form-invalid-feedback>
            </b-form-group>
            <b-form-group label="Description" label-align="left" label-class="font-weight-bold" :label-for="'input-'+task.id+'-description'">
                <b-form-textarea @change="onChange" :id="'input-'+task.id+'-description'" v-model="task.description" :state="descriptionState" placeholder="Description for the task."></b-form-textarea>
                        <b-form-invalid-feedback>
                    <template v-if="task.description && task.description.length > config.validation.maxDescription">
                        Max description length is {{ config.validation.maxDescription }} characters.
                    </template>
                </b-form-invalid-feedback>
            </b-form-group>
            <div class="row m-0">
            <b-form-group :class="ismodal ? 'col-12 p-0' :'col-md-4 p-0 pr-1'" label="Deadline" label-align="left" label-class="font-weight-bold" :label-for="'input-'+task.id+'-deadline'">
                <b-form-datepicker @input="onChange" :id="'input-'+task.id+'-deadline'" v-model="task.deadlineAt" :state="deadlineState" today-button  reset-button close-button></b-form-datepicker>
                        <b-form-invalid-feedback>
                    <template>Invalid deadline.</template>
                </b-form-invalid-feedback>
            </b-form-group>
            <b-form-group :class="ismodal ? 'col-12 p-0' : 'col-md-4 p-0 pr-1'" label="Priority" label-align="left" label-class="font-weight-bold" :label-for="'input-'+task.id+'-priority'">
                <b-form-input @change="onChange" :id="'input-'+task.id+'-priority'" :max="Number.MAX_SAFE_INTEGER" :min="Number.MIN_SAFE_INTEGER" v-model.number="task.priority" :state="priorityState" type="number"></b-form-input>
                <b-form-invalid-feedback>
                    <template>
                        Priority should be a numeric value between {{ Number.MIN_SAFE_INTEGER }} and {{ Number.MAX_SAFE_INTEGER }}.
                    </template>
                </b-form-invalid-feedback>
            </b-form-group>
            <b-form-group :class="ismodal ? 'col-12 p-0' : 'col-md-4 p-0'" label="Status" label-align="left" label-class="font-weight-bold" :label-for="'input-'+task.id+'-status'">
                <b-form-select @change="onChange" :id="'input-'+task.id+'-status'" v-model="task.status" :options="config.taskStates"></b-form-select>
                <b-form-invalid-feedback>
                    <template>
                        Priority should be a numeric value between {{ Number.MIN_SAFE_INTEGER }} and {{ Number.MAX_SAFE_INTEGER }}.
                    </template>
                </b-form-invalid-feedback>
            </b-form-group>
            </div>
        </b-form>
        <!-- Customize the buttons of modal -->
        <template v-if="ismodal" #modal-footer="{ok, cancel }">
            <div class="text-right">
                <b-button pill class="pr-4 pl-4" variant="primary" @click="ok">Save</b-button>
                <b-button pill class="ml-1" variant="secondary" @click="cancel">Cancel</b-button>
            </div>
        </template>
    </component>
</template>
<script lang="js">
import Vue from 'vue';
import tasks from '../config/tasks.js';
export default Vue.extend({
    name: 'task-editor',
    data() {
        return {
            config: {
                validation: {
                    maxSummary: 128,
                    maxDescription: 1024
                },
                taskStates: tasks.states.options
            }
        }
    },
    props: {
        id: String,
        task: { 
            type: Object, 
            default: function() { return { priority: 0 } } 
        },
        autosave: Boolean,
        ismodal: Boolean,
        title: String
    },
    computed: {
        summaryState() {
            if ( !this.task.summary ) return null;
            if ( this.task.summary.length > this.config.validation.maxSummary ) return false;
            return null;
        },
        descriptionState() {
            if ( !this.task.description ) return null;
            if ( this.task.description.length > this.config.validation.maxDescription ) return false;
            return null;
        },
        deadlineState() {
            if ( !this.task.deadlineAt ) return null;
            if ( isNaN(Date.parse(this.task.deadlineAt)) ) return false;
            return null;
        },
        priorityState() {
            if ( typeof this.task.priority !== 'number' ) return false;
            if ( !this.task.priority ) return null;
            if ( !Number.isInteger(this.task.priority) ) return false;
            if ( !Number.isSafeInteger(this.task.priority) ) return false;
            return null;
        },
        component() {
            if ( this.ismodal ) return 'b-modal';
            return 'div';
        }
    },
    methods: {
        // Input change
        onChange() {
            if (this.autosave) {
                this.save();
            }
        },
        save() {
            this.$emit('save', this.task, null);
        }
    }
});
</script>