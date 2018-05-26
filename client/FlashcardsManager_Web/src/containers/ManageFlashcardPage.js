import React from 'react';

import * as Ons from 'react-onsenui';
import ons from 'onsenui';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { colors } from '../util/colors';
import Toast from '../components/Toast';
import * as Actions from '../actions/manageFlashcards';
import FlashcardDetailsView from '../components/FlashcardDetailsView';
import BasePageWithBackButton from './base/BasePageWithBackButton';

class ManageFlashcardPage extends BasePageWithBackButton {
    constructor() {
        super();
        this.state = { openModal: false, name: ''};
    }

    renderFixed = () => {
            return (
                <Ons.Fab position='bottom right' onClick={this.handleDeleteClick}>
                    <Ons.Icon icon="md-delete" />
                </Ons.Fab>
            );
    }

    onDeleteClick = () => {
        const {deleteFlashcard} = this.props.actions;
        const {flashcardId} = this.props.manageFlashcards;
        const {navigator} = this.props;
        deleteFlashcard(flashcardId);
        navigator.popPage();
    }

    handleDeleteClick = () => {
        ons.notification.confirm({
            message: 'Czy na pewno chcesz usunąć tę fiszkę?',
            title: 'Potwierdź',
            buttonLabels: ['Nie', 'Tak'],
            callback: index => {
                if (index !== 1)
                    return;
                this.onDeleteClick();
            }
        });
    }

    renderPage() {
        const { flashcards, flashcardId } = this.props.manageFlashcards;
        const flashcard = flashcards.find(f => f.id === flashcardId);
        const labelStyle = { fontWeight: 'bold' };
        if(flashcard == null) {
            return null;
        }
        return (
            <div>
            <FlashcardDetailsView flashcard={flashcard} />
            {this.renderFixed()}
            </div>
        );
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(Actions, dispatch)
    };
};

const mapStateToProps = (state) => ({
    manageFlashcards: state.manageFlashcards
});

export default connect(mapStateToProps, mapDispatchToProps)(ManageFlashcardPage);