import React from 'react';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as Actions from '../actions';


import ons from 'onsenui';
import * as Ons from 'react-onsenui';

import BasePageWithBackButton from './base/BasePageWithBackButton';
import LearningFlashcardView from '../components/LearningFlashcardView';
import LearningSummaryView from '../components/LearningSummaryView';
import FetchingView from '../components/FetchingView';
import ErrorView from '../components/ErrorView';

import { flashcardResults } from '../api/types';


class LearningFlashcardPage extends BasePageWithBackButton {
    constructor() {
        super();
        this.state = { answers: { Success: 0, Partial: 0, Fail: 0 } };
    }
    isLearningEnded() {
        const { flashcards, flashcardIndex } = this.props.learning;
        return flashcards.length <= flashcardIndex;
    }
    handleBackButtonClick = () => {
        const { navigator } = this.props;
        if (this.isLearningEnded()) {
            navigator.popPage();
            return;
        }
        ons.notification.confirm({ message: 'Czy na pewno chcesz przerwać naukę?', buttonLabels: ['Nie', 'Tak'], title: 'Potwierdź' })
            .then((response) => {
                if (response === 1) {
                    navigator.popPage();
                }
            });
    };
    handleAnswerClick = (answer) => {
        const { actions } = this.props;
        const answers = { ...this.state.answers };
        switch (answer) {
            case flashcardResults["Success"]:
                answers.Success++;
                break;
            case flashcardResults["Partial"]:
                answers.Partial++;
                break;
            case flashcardResults["Fail"]:
                answers.Fail++;
                break;
        }
        this.setState({answers: answers});
        actions.proceedFlashcard(answer);
    }
    renderPage() {
        const { flashcards, flashcardIndex, isFetching, isInvalid } = this.props.learning;
        if (isInvalid) {
            return <ErrorView />;
        }
        const flashcard = flashcards[flashcardIndex];
        return (
            <div>
                {isFetching
                    ? <FetchingView/>
                    : this.renderView()
                }
            </div>
        );
    }

    renderView() {
        const { flashcards, flashcardIndex } = this.props.learning;
        const answers = this.state.answers;
        if (this.isLearningEnded()) {
            return <LearningSummaryView successCount={answers.Success} partialCount={answers.Partial} failCount={answers.Fail} />;
        };
        const flashcard = flashcards[flashcardIndex];
        return <LearningFlashcardView flashcard={flashcard} onAnswerClick={this.handleAnswerClick} />;

    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        actions: bindActionCreators(Actions, dispatch)
    };
};

const mapStateToProps = (state) => ({
    learning: state.learning
});

export default connect(mapStateToProps, mapDispatchToProps)(LearningFlashcardPage);