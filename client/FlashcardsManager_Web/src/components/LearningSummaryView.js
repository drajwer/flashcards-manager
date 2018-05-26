import React from 'react';

export default class LearningSummaryView extends React.Component {
    render() {
        const { successCount, partialCount, failCount } = this.props;
        return (
            <div style={{ padding: '10px 10px 60px 10px', margin: '30px 15px 25px 15px ' }} >
                <div className="card">
                    <h2 className="card__title" style={{ fontWeight: 'bold' }}>Ukończono naukę</h2>
                    <div className="card__content">
                        <p>Opanowane: {successCount}</p>
                        <p>Częściowo opanowane: {partialCount}</p>
                        <p>Nieopanowane: {failCount}</p>
                    </div>
                </div>
            </div>
        );
    }

}