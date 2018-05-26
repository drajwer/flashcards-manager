import React from 'react';

import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import * as Ons from 'react-onsenui';
import ons from 'onsenui';

import BasePageWithMenu from './base/BasePageWithMenu';
import CategoriesListView from '../components/CategoriesListView';
import ErrorView from '../components/ErrorView';
import FetchingView from '../components/FetchingView';

import * as Actions from '../actions/admin';
import { resetState } from '../actions/index';



class AdminMainPage extends BasePageWithMenu {
  constructor() {
    super();
    this.state = { selectedCategoryId: -1, showActionSheet: false }
  }
  resetState() {
    this.props.resetState();
  }
  componentDidMount() {
    const { categories, isInvalid } = this.props.admin;
    if (!isInvalid && categories.length > 0) {
      return;
    }
    this.props.actions.getAdminCategories();
  }
  handleCategorySelect = (categoryId) => {
    const { navigator, actions, categories } = this.props;
    this.setState({ selectedCategoryId: categoryId, showActionSheet: true })
  }
  handleDismissClick = () => {
    const { resetToast } = this.props.actions;
    resetToast();
  }
  handleActionSheetCancel = () => {
    this.setState({ showActionSheet: false });
  }

  handleRejectClick = () => {
    ons.notification.confirm({
      message: 'Czy na pewno chcesz odrzucić tę kategorię?',
      title: 'Potwierdź',
      buttonLabels: ['Nie', 'Tak'],
      callback: index => {
        if (index !== 1)
          return;
        this.props.actions.rejectCategory(this.state.selectedCategoryId);
      }
    });
    this.handleActionSheetCancel();
  }

  handleAcceptClick = () => {
    ons.notification.confirm({
      message: 'Czy na pewno chcesz zaakceptować tę kategorię?',
      title: 'Potwierdź',
      buttonLabels: ['Nie', 'Tak'],
      callback: index => {
        if (index !== 1)
          return;
        this.props.actions.acceptCategory(this.state.selectedCategoryId);
      }
    });
    this.handleActionSheetCancel();
  }

  renderPage() {
    const { categories, isFetching, isInvalid, showToast, message } = this.props.admin;
    if (isInvalid) {
      return <div>Nie masz uprawnień do panelu administratora.</div>
    };
    return (
      <div>
        {
          isFetching
            ? <FetchingView />

            : this.renderView()
        }
      </div>
    )
  }

  renderView() {
    const { categories, showToast, message } = this.props.admin;
    const {showActionSheet} = this.state;
    return (
      <Ons.Page>
        <CategoriesListView
          title="Kategorie do zaakceptowania"
          onCategorySelect={this.handleCategorySelect}
          categories={categories}
          showToast={showToast}
          message={message}
          onDismissClick={this.handleDismissClick}
        />
        <Ons.ActionSheet isOpen={showActionSheet}
          onCancel={this.handleActionSheetCancel}
          isCancelable
          title={'Co chcesz zrobić?'}
        >
          <Ons.ActionSheetButton onClick={this.handleAcceptClick} icon={'fa-check'}>Zaakceptuj</Ons.ActionSheetButton>
          <Ons.ActionSheetButton onClick={this.handleRejectClick} icon={'fa-times'}>Odrzuć</Ons.ActionSheetButton>
          <Ons.ActionSheetButton onClick={this.handleActionSheetCancel} icon={'md-close'}>Powrót</Ons.ActionSheetButton>
        </Ons.ActionSheet>
      </Ons.Page>
    );
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(Actions, dispatch),
    resetState: bindActionCreators(resetState, dispatch)
  };
};

const mapStateToProps = (state) => ({
  admin: state.admin
});

export default connect(mapStateToProps, mapDispatchToProps)(AdminMainPage);